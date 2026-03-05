#!/bin/bash
set +e
set -x

# ============================================================================
# VPS Bootstrap Script - Idempotent & Configurable
# ============================================================================
# This script can be run multiple times safely to ensure a known working state.
# It doesn't skip steps - instead, operations are naturally idempotent.
# All configuration can be provided via environment variables or config file.
# ============================================================================

# Enable debug mode if requested
[ "${DEBUG:-false}" = "true" ] && set -x

SCRIPT_DIR=$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)

# ============================================================================
# CONFIGURATION - Override with environment variables or config file
# ============================================================================

# Load defaults/secrets file first (legacy defaults profile), then runtime config.
DEFAULTS_FILE="${BOOTSTRAP_DEFAULTS_FILE:-${SCRIPT_DIR}/bootstrap-secrets.defaults.env}"
if [ -f "$DEFAULTS_FILE" ]; then
  if grep -qP '\r$' "$DEFAULTS_FILE" 2>/dev/null; then
    source <(sed 's/\r$//' "$DEFAULTS_FILE")
  else
    source "$DEFAULTS_FILE"
  fi
fi

# Load config file if it exists
CONFIG_FILE="${BOOTSTRAP_CONFIG_FILE:-/etc/bootstrap-config.env}"
if [ -f "$CONFIG_FILE" ]; then
  if grep -qP '\r$' "$CONFIG_FILE" 2>/dev/null; then
    source <(sed 's/\r$//' "$CONFIG_FILE")
  else
    source "$CONFIG_FILE"
  fi
fi

# Core Configuration
DOMAIN="${DOMAIN:-example.com}"
HOSTNAME_ARG="${1:-${BOOTSTRAP_HOSTNAME:-$(hostname -s 2>/dev/null || hostname | cut -d'.' -f1)}}"
HOSTNAME_SHORT=$(echo "$HOSTNAME_ARG" | cut -d'.' -f1)
FQDN="${HOSTNAME_SHORT}.${DOMAIN}"

# User Configuration
PRIMARY_USER="${PRIMARY_USER:-ubuntu}"
ADMIN_USERS="${ADMIN_USERS:-root ubuntu}"
PASSWORD_HASH="${PASSWORD_HASH:-}"
GITHUB_USERS="${GITHUB_USERS:-}" # Comma-separated list

# DNS Configuration
DNS_SERVERS="${DNS_SERVERS:-1.1.1.1,1.0.0.1,8.8.8.8,8.8.4.4}"
IFS=',' read -ra DNS_ARRAY <<<"$DNS_SERVERS"

# Tailscale Configuration
ENABLE_TAILSCALE="${ENABLE_TAILSCALE:-true}"
TAILSCALE_AUTH_KEY="${TAILSCALE_AUTH_KEY:-}"
TAILSCALE_LOGIN_SERVER="${TAILSCALE_LOGIN_SERVER:-https://headscale.${DOMAIN}}"
TAILSCALE_ADVERTISE_EXIT="${TAILSCALE_ADVERTISE_EXIT:-true}"

# Docker Configuration
ENABLE_DOCKER="${ENABLE_DOCKER:-true}"
DOCKER_VERSION="${DOCKER_VERSION:-27.0}"

# Stack Deployment Configuration
ENABLE_STACK_DEPLOY="${ENABLE_STACK_DEPLOY:-true}"
STACK_INIT_SCRIPTS="${STACK_INIT_SCRIPTS:-scripts/crowdsec-bootstrap.sh}"
STACK_COMPOSE_FILE="${STACK_COMPOSE_FILE:-docker-compose.yml}"
STACK_COMPOSE_UP_ARGS="${STACK_COMPOSE_UP_ARGS:---remove-orphans --pull never --no-build --no-deps}"
STACK_EXCLUDE_SERVICES="${STACK_EXCLUDE_SERVICES:-crowdsec-init telemetry-auth bolabaden-nextjs logrotate-traefik init_victoriametrics init_prometheus}"  # Build-only or missing images

# Nomad/Consul Configuration
ENABLE_NOMAD="${ENABLE_NOMAD:-true}"
ENABLE_CONSUL="${ENABLE_CONSUL:-true}"
NOMAD_DATACENTER="${NOMAD_DATACENTER:-dc1}"
NOMAD_BOOTSTRAP_EXPECT="${NOMAD_BOOTSTRAP_EXPECT:-1}"
NOMAD_NODE_CLASS="${NOMAD_NODE_CLASS:-balanced}"
NOMAD_SERVERS="${NOMAD_SERVERS:-}" # Comma-separated IPs, auto-detected if empty

# Swap Configuration
SWAP_SIZE="${SWAP_SIZE:-4G}"
SWAP_FILE="${SWAP_FILE:-/swapfile}"

# SSH Configuration
SSH_PERMIT_ROOT="${SSH_PERMIT_ROOT:-yes}"
SSH_PASSWORD_AUTH="${SSH_PASSWORD_AUTH:-yes}"

# Timezone Configuration
TZ="${TZ:-}" # Auto-detect via GeoIP if empty

# Storage Maintenance Configuration
ENABLE_STORAGE_MAINTENANCE="${ENABLE_STORAGE_MAINTENANCE:-true}"
MAINTENANCE_TMP_RETENTION_DAYS="${MAINTENANCE_TMP_RETENTION_DAYS:-10}"
MAINTENANCE_JOURNAL_RETENTION="${MAINTENANCE_JOURNAL_RETENTION:-14d}"
MAINTENANCE_JOURNAL_MAX_SIZE="${MAINTENANCE_JOURNAL_MAX_SIZE:-1G}"
MAINTENANCE_DOCKER_PRUNE_UNTIL="${MAINTENANCE_DOCKER_PRUNE_UNTIL:-720h}"
MAINTENANCE_DOCKER_BUILDER_KEEP_STORAGE="${MAINTENANCE_DOCKER_BUILDER_KEEP_STORAGE:-20GB}"
MAINTENANCE_DAILY_CRON="${MAINTENANCE_DAILY_CRON:-17 3 * * *}"
MAINTENANCE_WEEKLY_CRON="${MAINTENANCE_WEEKLY_CRON:-42 4 * * 0}"
MAINTENANCE_PRESSURE_CRON="${MAINTENANCE_PRESSURE_CRON:-27 * * * *}"
MAINTENANCE_PRESSURE_FREE_GB="${MAINTENANCE_PRESSURE_FREE_GB:-20}"

# ============================================================================
# HELPER FUNCTIONS
# ============================================================================

log() {
  echo "[$(date +'%Y-%m-%d %H:%M:%S')] $*"
}

log_info() {
  log "[*] $*"
}

log_success() {
  log "  [OK] $*"
}

log_warn() {
  log "  [WARN] $*"
}

log_error() {
  log "  [ERROR] $*"
}

# ============================================================================
# MAIN SCRIPT
# ============================================================================

log_info "========================================"
log_info "VPS Bootstrap Script Starting"
log_info "========================================"
log_info "Hostname: $HOSTNAME_SHORT"
log_info "Domain: $DOMAIN"
log_info "FQDN: $FQDN"
log_info "========================================"

# Check if running as root
if [ "$EUID" -ne 0 ]; then
  log_error "This script must be run as root"
  exit 1
fi

# ============================================================================
# HOSTNAME CONFIGURATION
# ============================================================================

log_info "Configuring hostname..."
hostnamectl set-hostname "$HOSTNAME_SHORT"
echo "$HOSTNAME_SHORT" >/etc/hostname

# Update /etc/hosts - remove existing entries and add new one
sed -i "/127.0.1.1.*${HOSTNAME_SHORT}/d" /etc/hosts
if ! grep -q "127.0.1.1 ${FQDN} ${HOSTNAME_SHORT}" /etc/hosts; then
  echo "127.0.1.1 ${FQDN} ${HOSTNAME_SHORT}" >>/etc/hosts
fi
log_success "Hostname configured"

# ============================================================================
# SYSTEM UPDATE
# ============================================================================

log_info "Updating system packages..."
apt-get update -qq
apt-get autoremove -y -qq
DEBIAN_FRONTEND=noninteractive apt-get upgrade -y -qq
log_success "System packages updated"

# ============================================================================
# ESSENTIAL PACKAGES
# ============================================================================

log_info "Installing essential packages..."
DEBIAN_FRONTEND=noninteractive apt-get install -y -qq \
  curl wget git htop nano vim unzip jq bc yq \
  iptables-persistent python3 python3-pip \
  nodejs npm python3-venv pipx plocate whois sshpass ansible \
  dnsutils bind9-host ethtool ca-certificates gnupg lsb-release 2>&1 | grep -v "^Preconfiguring" || true

apt-get autoremove -y -qq
log_success "Essential packages installed"

# ============================================================================
# DOCKER INSTALLATION
# ============================================================================

if [ "$ENABLE_DOCKER" = "true" ]; then
  log_info "Installing Docker..."

  # Remove conflicting packages
  DEBIAN_FRONTEND=noninteractive apt-get remove -y -qq docker.io docker-doc docker-compose docker-compose-v2 podman-docker containerd runc 2>/dev/null || true

  # Clean up existing Docker sources
  rm -f /etc/apt/sources.list.d/docker.list /etc/apt/sources.list.d/docker.sources

  # Add Docker's official GPG key
  install -m 0755 -d /etc/apt/keyrings
  curl -fsSL https://download.docker.com/linux/ubuntu/gpg -o /etc/apt/keyrings/docker.asc
  chmod a+r /etc/apt/keyrings/docker.asc

  # Add repository
  echo "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.asc] https://download.docker.com/linux/ubuntu $(. /etc/os-release && echo "${UBUNTU_CODENAME:-$VERSION_CODENAME}") stable" |
    tee /etc/apt/sources.list.d/docker.list >/dev/null

  apt-get update -qq
  DEBIAN_FRONTEND=noninteractive apt-get install -y -qq docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin

  # Clean up duplicates
  [ -f /etc/apt/sources.list.d/docker.sources ] && rm -f /etc/apt/sources.list.d/docker.list

  # Enable and start Docker
  systemctl enable docker.service containerd.service
  systemctl start docker.service containerd.service

  log_success "Docker installed: $(docker --version)"

  # Configure Docker daemon defaults (log rotation + BuildKit GC)
  log_info "Configuring /etc/docker/daemon.json..."
  mkdir -p /etc/docker
  # Merge with existing daemon.json if present, otherwise create fresh
  if [ ! -f /etc/docker/daemon.json ] || ! python3 -c 'import json,sys; json.load(open(sys.argv[1]))' /etc/docker/daemon.json 2>/dev/null; then
    cat >/etc/docker/daemon.json <<'DAEMON_EOF'
{
  "log-driver": "json-file",
  "log-opts": {
    "max-size": "20m",
    "max-file": "3"
  },
  "builder": {
    "gc": {
      "enabled": true,
      "defaultKeepStorage": "5GB",
      "policy": [
        {"keepStorage": "5GB", "filter": ["unused-for=72h"]},
        {"keepStorage": "10GB", "all": true}
      ]
    }
  }
}
DAEMON_EOF
  else
    # Ensure log-driver and builder.gc are set even if daemon.json already exists
    python3 -c '
import json, sys
with open("/etc/docker/daemon.json") as f:
    cfg = json.load(f)
cfg.setdefault("log-driver", "json-file")
cfg.setdefault("log-opts", {})
cfg["log-opts"].setdefault("max-size", "20m")
cfg["log-opts"]["max-file"] = "3"
cfg.setdefault("builder", {})
cfg["builder"]["gc"] = {
    "enabled": True,
    "defaultKeepStorage": "5GB",
    "policy": [
        {"keepStorage": "5GB", "filter": ["unused-for=72h"]},
        {"keepStorage": "10GB", "all": True}
    ]
}
with open("/etc/docker/daemon.json", "w") as f:
    json.dump(cfg, f, indent=2)
' 2>/dev/null || true
  fi
  systemctl restart docker.service 2>/dev/null || true
  log_success "Docker daemon.json configured"

  # Create size-aware image tag retention script
  log_info "Installing size-aware image retention script..."
  cat >/usr/local/sbin/docker-image-retention.sh <<'IMAGE_RETENTION_EOF'
#!/bin/bash
# Smart image retention: Keep 2 tags normally, remove largest backups first in emergency
set -euo pipefail

KEEP_TAGS="${DOCKER_KEEP_TAGS:-2}"
EMERGENCY_MODE="${1:-normal}"  # "normal" or "emergency"

remove_backup_tags_for_image() {
  local repo="$1"
  [ -z "$repo" ] || [ "$repo" = "<none>" ] && return 0

  mapfile -t tags < <(docker images "$repo" --format "{{.Tag}}@{{.CreatedAt}}" | sort -t'@' -k2 -r 2>/dev/null || true)
  [ "${#tags[@]}" -le 1 ] && return 0

  # Remove all but latest (backup removal mode)
  for ((i=1; i<${#tags[@]}; i++)); do
    tag="${tags[$i]%%@*}"
    [ "$tag" = "<none>" ] || [ "$tag" = "latest" ] && continue
    echo "  Removing backup: $repo:$tag"
    docker rmi "$repo:$tag" 2>/dev/null || true
  done
}

if [ "$EMERGENCY_MODE" = "emergency" ]; then
  echo "[EMERGENCY] Size-aware cleanup: Removing backups of LARGEST images first..."

  # Get repos ranked by image count (proxy for size)
  docker images --format "{{.Repository}}" | sort -u | while read -r repo; do
    if [ ! -z "$repo" ] && [ "$repo" != "<none>" ]; then
      count=$(docker images "$repo" --quiet 2>/dev/null | wc -l || echo 0)
      echo "$count $repo"
    fi
  done | sort -rn | awk '{print $2}' | head -20 | while read -r repo; do
    remove_backup_tags_for_image "$repo"
  done
else
  # Normal mode: Keep KEEP_TAGS per repo
  docker images --format "{{.Repository}}" | sort -u | while read -r repo; do
    [ -z "$repo" ] || [ "$repo" = "<none>" ] && continue

    mapfile -t tags < <(docker images "$repo" --format "{{.Tag}}@{{.CreatedAt}}" | sort -t'@' -k2 -r 2>/dev/null || true)
    [ "${#tags[@]}" -le "$KEEP_TAGS" ] && continue

    for ((i=KEEP_TAGS; i<${#tags[@]}; i++)); do
      tag="${tags[$i]%%@*}"
      [ "$tag" = "<none>" ] || [ "$tag" = "latest" ] && continue
      echo "Removing old tag: $repo:$tag"
      docker rmi "$repo:$tag" 2>/dev/null || true
    done
  done
fi

docker image prune -f >/dev/null 2>&1 || true
IMAGE_RETENTION_EOF
  chmod 0755 /usr/local/sbin/docker-image-retention.sh
  log_success "Size-aware image retention script installed"

  # Docker Hub login  (if credentials are provided)
  if [ -n "$DOCKERHUB_USERNAME" ] && [ -n "$DOCKERHUB_TOKEN" ] && [ "$DOCKERHUB_TOKEN" != "changeme" ]; then
    log_info "Logging into Docker Hub..."
    echo "$DOCKERHUB_TOKEN" | docker login -u "$DOCKERHUB_USERNAME" --password-stdin >/dev/null 2>&1 && \
      log_success "Docker Hub authentication successful" || \
      log_warn "Docker Hub login failed (continuing without authentication)"
  elif [ -n "$DOCKERHUB_USERNAME" ] && [ -n "$DOCKERHUB_PASSWORD" ] && [ "$DOCKERHUB_PASSWORD" != "changeme" ]; then
    log_info "Logging into Docker Hub..."
    echo "$DOCKERHUB_PASSWORD" | docker login -u "$DOCKERHUB_USERNAME" --password-stdin >/dev/null 2>&1 && \
      log_success "Docker Hub authentication successful" || \
      log_warn "Docker Hub login failed (continuing without authentication)"
  else
    log_warn "No Docker Hub credentials provided; rate limits may apply to image pulls"
  fi
fi

# ============================================================================
# SSH CONFIGURATION
# ============================================================================

log_info "Configuring SSH..."

# Backup SSH config if it's the first time or weekly
BACKUP_COUNT=$(find /etc/ssh -name "sshd_config.backup.*" 2>/dev/null | wc -l)
OLD_BACKUP_COUNT=$(find /etc/ssh -name "sshd_config.backup.*" -mtime +7 2>/dev/null | wc -l)
if [ "$BACKUP_COUNT" -eq 0 ] || [ "$OLD_BACKUP_COUNT" -gt 0 ]; then
  cp /etc/ssh/sshd_config /etc/ssh/sshd_config.backup.$(date +%Y%m%d_%H%M%S)
fi

# Configure SSH settings
if grep -qE '^\s*PasswordAuthentication\s+' /etc/ssh/sshd_config; then
  sed -i "s/^\s*PasswordAuthentication\s.*/PasswordAuthentication $SSH_PASSWORD_AUTH/" /etc/ssh/sshd_config
else
  echo "PasswordAuthentication $SSH_PASSWORD_AUTH" >>/etc/ssh/sshd_config
fi

if grep -qE '^\s*PubkeyAuthentication\s+' /etc/ssh/sshd_config; then
  sed -i "s/^\s*PubkeyAuthentication\s.*/PubkeyAuthentication yes/" /etc/ssh/sshd_config
else
  echo "PubkeyAuthentication yes" >>/etc/ssh/sshd_config
fi

if grep -qE '^\s*PermitRootLogin\s+' /etc/ssh/sshd_config; then
  sed -i "s/^\s*PermitRootLogin\s.*/PermitRootLogin $SSH_PERMIT_ROOT/" /etc/ssh/sshd_config
else
  echo "PermitRootLogin $SSH_PERMIT_ROOT" >>/etc/ssh/sshd_config
fi

# Detect SSH service name (Ubuntu 24.04+ uses 'ssh', older uses 'sshd')
SSH_SERVICE="sshd"
if systemctl list-unit-files ssh.service >/dev/null 2>&1 && ! systemctl list-unit-files sshd.service >/dev/null 2>&1; then
  SSH_SERVICE="ssh"
fi

# Test and reload SSH
if sshd -t 2>/dev/null; then
  systemctl reload "$SSH_SERVICE" || systemctl restart "$SSH_SERVICE" || log_warn "Could not reload/restart SSH service"
  log_success "SSH configured"
else
  log_warn "SSH configuration test failed; skipping reload"
fi

# ============================================================================
# USER CONFIGURATION
# ============================================================================

log_info "Configuring users..."

for USER in $ADMIN_USERS; do
  log_info "  Setting up user: $USER"

  # Create user if doesn't exist
  if ! id "$USER" &>/dev/null; then
    useradd -m -s /bin/bash -G sudo "$USER" 2>/dev/null || true
  else
    usermod -aG sudo "$USER" 2>/dev/null || true
  fi

  # Add to docker group if Docker is enabled
  if [ "$ENABLE_DOCKER" = "true" ]; then
    usermod -aG docker "$USER" 2>/dev/null || true
  fi

  HOME_DIR=$(eval echo "~$USER")

  # Setup Docker directory
  if [ "$ENABLE_DOCKER" = "true" ]; then
    mkdir -p "$HOME_DIR/.docker"
    chown "$USER":"$USER" "$HOME_DIR/.docker" -R 2>/dev/null || true
    chmod g+rwx "$HOME_DIR/.docker" -R 2>/dev/null || true
  fi

  # Setup SSH keys
  mkdir -p "$HOME_DIR/.ssh"
  chmod 700 "$HOME_DIR/.ssh"
  chown "$USER":"$USER" "$HOME_DIR/.ssh"
  touch "$HOME_DIR/.ssh/authorized_keys"
  chmod 600 "$HOME_DIR/.ssh/authorized_keys"

  # Import SSH keys from GitHub
  IFS=',' read -ra GH_USERS <<<"$GITHUB_USERS"
  for gh_user in "${GH_USERS[@]}"; do
    gh_user=$(echo "$gh_user" | xargs) # Trim whitespace
    if [ -n "$gh_user" ]; then
      GITHUB_KEYS=$(curl -fsSL --max-time 10 "https://github.com/${gh_user}.keys" 2>/dev/null || true)
      if [ -n "$GITHUB_KEYS" ]; then
        while IFS= read -r key; do
          [ -n "$key" ] && ! grep -Fxq "$key" "$HOME_DIR/.ssh/authorized_keys" 2>/dev/null && echo "$key" >>"$HOME_DIR/.ssh/authorized_keys"
        done <<<"$GITHUB_KEYS"
      fi
    fi
  done

  chown "$USER":"$USER" "$HOME_DIR/.ssh/authorized_keys" 2>/dev/null || true

  # Set password
  if [ -n "$PASSWORD_HASH" ]; then
    echo "$USER:${PASSWORD_HASH}" | chpasswd -e 2>/dev/null || true
  fi

  # Setup pipx and uv
  sudo -u "$USER" -H bash -c 'pipx ensurepath 2>/dev/null || true' 2>/dev/null || true
  sudo -u "$USER" -H bash -c 'pipx list | grep -q uv || pipx install uv 2>/dev/null' 2>/dev/null || true
done

log_success "Users configured"

# ============================================================================
# INFRA REPO + DOCKER NETWORK BOOTSTRAP
# ============================================================================

log_info "Bootstrapping infra repo and Docker network..."

# Resolve stack user home dynamically (actual user, not hardcoded ubuntu).
STACK_USER="${BOOTSTRAP_STACK_USER:-$PRIMARY_USER}"
STACK_HOME="$(getent passwd "$STACK_USER" 2>/dev/null | cut -d: -f6)"
[ -z "$STACK_HOME" ] && STACK_HOME="/home/$STACK_USER"
STACK_DIR="${BOOTSTRAP_STACK_DIR:-$STACK_HOME/my-media-stack}"

# Clone/update bolabaden-infra repository idempotently.
mkdir -p "$(dirname "$STACK_DIR")"
if [ -d "$STACK_DIR/.git" ]; then
  ORIGIN_URL="$(git -C "$STACK_DIR" config --get remote.origin.url 2>/dev/null || true)"
  if echo "$ORIGIN_URL" | grep -Eq 'github.com[:/]+bolabaden/bolabaden-infra(\.git)?$'; then
    git -C "$STACK_DIR" pull --ff-only >/dev/null 2>&1 || log_warn "Could not fast-forward update $STACK_DIR"
  else
    log_warn "Existing repo at $STACK_DIR is not bolabaden/bolabaden-infra; skipping git clone"
  fi
else
  if [ -d "$STACK_DIR" ] && [ "$(find "$STACK_DIR" -mindepth 1 -maxdepth 1 2>/dev/null | wc -l)" -gt 0 ]; then
    log_warn "$STACK_DIR exists and is not empty; skipping git clone"
  else
    git clone https://github.com/bolabaden/bolabaden-infra "$STACK_DIR" >/dev/null 2>&1 || log_warn "Could not clone bolabaden-infra"
  fi
fi

# Ensure ownership for non-root primary users.
if id "$STACK_USER" >/dev/null 2>&1; then
  chown -R "$STACK_USER":"$STACK_USER" "$STACK_DIR" 2>/dev/null || true
fi

# Ensure warp-nat-net exists with the expected bridge name and subnet defaults.
if [ "$ENABLE_DOCKER" = "true" ] && command -v docker >/dev/null 2>&1; then
  if ! docker network inspect warp-nat-net >/dev/null 2>&1; then
    docker network create --attachable \
      -o com.docker.network.bridge.name=br_warp-nat-net \
      -o com.docker.network.bridge.enable_ip_masquerade=false \
      warp-nat-net \
      --subnet="${WARP_NAT_NET_SUBNET:-10.0.2.0/24}" \
      --gateway="${WARP_NAT_NET_GATEWAY:-10.0.2.1}" >/dev/null 2>&1 || log_warn "Could not create warp-nat-net"
  fi
fi

log_success "Infra repo and Docker network bootstrap complete"

# ============================================================================
# NETWORK OPTIMIZATION
# ============================================================================

log_info "Optimizing network settings..."

# Detect primary interface
IFACE="$(ip route show default 0.0.0.0/0 | awk '{print $5}' | head -n1)"
if [ -n "$IFACE" ]; then
  log_info "  Primary interface: $IFACE"

  # Try to optimize NIC settings
  ethtool -K "$IFACE" gro on 2>/dev/null || true
  ethtool -K "$IFACE" lro off 2>/dev/null || true
  ethtool -K "$IFACE" rx-udp-gro-forwarding on 2>/dev/null || true
  ethtool -K "$IFACE" tx-udp-segmentation on 2>/dev/null || true
fi

# Enable IP forwarding
cat >/etc/sysctl.d/99-forwarding.conf <<EOF
net.ipv4.ip_forward = 1
net.ipv6.conf.all.forwarding = 1
EOF
sysctl -p /etc/sysctl.d/99-forwarding.conf || true

log_success "Network settings optimized"

# ============================================================================
# DNS CONFIGURATION
# ============================================================================

log_info "Configuring DNS..."

mkdir -p /etc/systemd/resolved.conf.d
cat >/etc/systemd/resolved.conf.d/custom.conf <<EOF
[Resolve]
DNS=${DNS_ARRAY[0]} ${DNS_ARRAY[1]:-}
FallbackDNS=${DNS_ARRAY[2]:-} ${DNS_ARRAY[3]:-}
DNSOverTLS=opportunistic
DNSSEC=allow-downgrade
EOF

if systemctl is-active --quiet systemd-resolved; then
  systemctl restart systemd-resolved
else
  # Fallback to /etc/resolv.conf
  : >/etc/resolv.conf
  for dns in "${DNS_ARRAY[@]}"; do
    echo "nameserver $dns" >>/etc/resolv.conf
  done
fi

log_success "DNS configured"

# ============================================================================
# TAILSCALE
# ============================================================================

if [ "$ENABLE_TAILSCALE" = "true" ]; then
  log_info "Installing and configuring Tailscale..."

  # Install if not present
  if ! command -v tailscale >/dev/null 2>&1; then
    curl -fsSL https://tailscale.com/install.sh | sh
  fi

  # Configure
  tailscale down 2>/dev/null || true

  if [ -z "$TAILSCALE_AUTH_KEY" ]; then
    log_warn "TAILSCALE_AUTH_KEY is empty; tailscale up may require interactive login"
  fi

  TS_CMD="tailscale up --login-server=${TAILSCALE_LOGIN_SERVER} --hostname=${HOSTNAME_SHORT} --operator=${PRIMARY_USER} --accept-dns=true"
  [ -n "$TAILSCALE_AUTH_KEY" ] && TS_CMD+=" --auth-key=${TAILSCALE_AUTH_KEY}"
  [ "$TAILSCALE_ADVERTISE_EXIT" = "true" ] && TS_CMD+=" --advertise-exit-node"
  TS_CMD+=" --reset"

  eval "$TS_CMD" || log_warn "Tailscale setup may need manual intervention"

  sleep 3
  tailscale status || true

  log_success "Tailscale configured"
fi

# ============================================================================
# NODE.JS TOOLS
# ============================================================================

log_info "Installing Node.js tools..."

if [ ! -f /usr/local/bin/n ]; then
  curl -fsSL https://raw.githubusercontent.com/tj/n/master/bin/n -o /usr/local/bin/n
  chmod +x /usr/local/bin/n
fi

/usr/local/bin/n lts 2>/dev/null || log_warn "Could not install Node.js LTS"

log_success "Node.js tools installed"

# ============================================================================
# SECURITY UPDATES
# ============================================================================

log_info "Configuring automatic security updates..."

cat >/etc/apt/apt.conf.d/20auto-upgrades <<EOF
APT::Periodic::Update-Package-Lists "1";
APT::Periodic::Unattended-Upgrade "1";
EOF

log_success "Automatic updates configured"

# ============================================================================
# TIMEZONE
# ============================================================================

log_info "Configuring timezone..."

if [ -n "$TZ" ]; then
  timedatectl set-timezone "$TZ"
  log_info "  Timezone set to: $TZ"
else
  # Auto-detect via GeoIP
  TZ_GEO=$(curl -fsSL --max-time 5 https://ipapi.co/timezone 2>/dev/null || curl -fsSL --max-time 5 https://ipinfo.io/timezone 2>/dev/null || true)
  if [ -n "$TZ_GEO" ]; then
    timedatectl set-timezone "$TZ_GEO"
    log_info "  Timezone auto-detected: $TZ_GEO"
  else
    log_warn "Could not determine timezone"
  fi
fi

log_success "Timezone configured"

# ============================================================================
# NOMAD & CONSUL
# ============================================================================

if [ "$ENABLE_NOMAD" = "true" ] || [ "$ENABLE_CONSUL" = "true" ]; then
  log_info "Installing HashiCorp tools..."

  # Add HashiCorp repository
  rm -f /usr/share/keyrings/hashicorp-archive-keyring.gpg 2>/dev/null || true
  apt-get update -qq
  apt-get install -y -qq wget gpg coreutils

  log_info "  Adding HashiCorp GPG key..."
  wget -4 -qO- https://apt.releases.hashicorp.com/gpg | gpg --dearmor -o /usr/share/keyrings/hashicorp-archive-keyring.gpg 2>/dev/null

  echo "deb [signed-by=/usr/share/keyrings/hashicorp-archive-keyring.gpg] https://apt.releases.hashicorp.com $(lsb_release -cs) main" |
    tee /etc/apt/sources.list.d/hashicorp.list >/dev/null

  apt-get update -qq

  # Install requested packages
  PACKAGES=()
  [ "$ENABLE_NOMAD" = "true" ] && PACKAGES+=("nomad")
  [ "$ENABLE_CONSUL" = "true" ] && PACKAGES+=("consul" "consul-cni")

  apt-get install -y -qq "${PACKAGES[@]}"

  # Install CNI plugins if Nomad is enabled
  if [ "$ENABLE_NOMAD" = "true" ] && [ ! -d /opt/cni/bin ]; then
    log_info "  Installing CNI plugins..."
    ARCH_CNI=$([ "$(uname -m)" = "aarch64" ] && echo "arm64" || echo "amd64")
    CNI_VERSION="v1.6.2"

    curl -fsSL -o /tmp/cni-plugins.tgz "https://github.com/containernetworking/plugins/releases/download/${CNI_VERSION}/cni-plugins-linux-${ARCH_CNI}-${CNI_VERSION}.tgz"
    mkdir -p /opt/cni/bin
    tar -C /opt/cni/bin -xzf /tmp/cni-plugins.tgz
    rm -f /tmp/cni-plugins.tgz
  fi

  # Configure bridge networking
  if [ "$ENABLE_NOMAD" = "true" ]; then
    modprobe br_netfilter 2>/dev/null || true

    cat >/etc/sysctl.d/bridge.conf <<EOF
net.bridge.bridge-nf-call-arptables = 1
net.bridge.bridge-nf-call-ip6tables = 1
net.bridge.bridge-nf-call-iptables = 1
EOF
    sysctl --system >/dev/null 2>&1 || true
  fi

  log_success "HashiCorp tools installed"
fi

# ============================================================================
# NOMAD CONFIGURATION
# ============================================================================

if [ "$ENABLE_NOMAD" = "true" ]; then
  log_info "Configuring Nomad..."

  mkdir -p /nomad/{data,log} /etc/nomad.d

  # Auto-discover Nomad servers if not provided
  if [ -z "$NOMAD_SERVERS" ] && command -v tailscale >/dev/null 2>&1; then
    DISCOVERED_SERVERS=$(tailscale status --json 2>/dev/null | jq -r '.Peer[].TailscaleIPs[0]' 2>/dev/null || true)
    if [ -n "$DISCOVERED_SERVERS" ]; then
      NOMAD_SERVERS=$(echo "$DISCOVERED_SERVERS" | tr '\n' ',' | sed 's/,$//')
    fi
  fi

  # Build retry_join array
  RETRY_JOIN_CONFIG=""
  if [ -n "$NOMAD_SERVERS" ]; then
    IFS=',' read -ra SERVER_ARRAY <<<"$NOMAD_SERVERS"
    RETRY_JOIN_CONFIG="    retry_join = ["
    for server in "${SERVER_ARRAY[@]}"; do
      server=$(echo "$server" | xargs)
      [ -n "$server" ] && RETRY_JOIN_CONFIG="${RETRY_JOIN_CONFIG}\n      \"${server}:4647\","
    done
    RETRY_JOIN_CONFIG="${RETRY_JOIN_CONFIG%,}\n    ]"
  fi

  # Get private IP
  PRIVATE_IP=$(ip route get 1.1.1.1 | awk '{print $7; exit}' 2>/dev/null || echo "{{ GetPrivateIP }}")

  cat >/etc/nomad.d/nomad.hcl <<EOF
datacenter = "${NOMAD_DATACENTER}"
data_dir = "/nomad/data/"
bind_addr = "0.0.0.0"
log_level = "INFO"
log_json = true
log_file = "/nomad/log/nomad.log"
log_rotate_bytes = 10485760
log_rotate_max_files = 5

server {
  enabled = true
  bootstrap_expect = ${NOMAD_BOOTSTRAP_EXPECT}
$([ -n "$RETRY_JOIN_CONFIG" ] && echo -e "  server_join {\n$RETRY_JOIN_CONFIG\n  }")
}

client {
  enabled = true
  node_class = "${NOMAD_NODE_CLASS}"
}

advertise {
  http = "${PRIVATE_IP}:4646"
  rpc  = "${PRIVATE_IP}:4647"
  serf = "${PRIVATE_IP}:4648"
}

$(
    [ "$ENABLE_CONSUL" = "true" ] && cat <<CONSUL
consul {
  address = "127.0.0.1:8500"
  auto_advertise = true
  server_service_name = "nomad"
  client_service_name = "nomad-client"
}
CONSUL
  )

telemetry {
  collection_interval = "1s"
  publish_allocation_metrics = true
}
EOF

  # Enable and start Nomad
  systemctl daemon-reload
  systemctl enable nomad
  systemctl restart nomad

  sleep 5
  nomad server members 2>/dev/null || true

  log_success "Nomad configured"
fi

# ============================================================================
# SWAP CONFIGURATION
# ============================================================================

log_info "Configuring swap..."

if [ ! -f "$SWAP_FILE" ]; then
  fallocate -l "$SWAP_SIZE" "$SWAP_FILE" 2>/dev/null || dd if=/dev/zero of="$SWAP_FILE" bs=1M count=$(echo "$SWAP_SIZE" | sed 's/G$//' | awk '{print $1*1024}') 2>/dev/null
  chmod 600 "$SWAP_FILE"
  mkswap "$SWAP_FILE"
  swapon "$SWAP_FILE"

  # Add to fstab if not present
  if ! grep -q "$SWAP_FILE" /etc/fstab; then
    echo "${SWAP_FILE} none swap sw 0 0" >>/etc/fstab
  fi

  log_success "Swap file created: $SWAP_SIZE"
else
  swapon "$SWAP_FILE" 2>/dev/null || log_info "  Swap already active"
fi

# ============================================================================
# STORAGE MAINTENANCE (CRON + LOG RETENTION)
# ============================================================================

if [ "$ENABLE_STORAGE_MAINTENANCE" = "true" ]; then
  log_info "Configuring automated storage maintenance..."

  # Keep journald bounded so logs cannot consume the full disk.
  mkdir -p /etc/systemd/journald.conf.d
  cat >/etc/systemd/journald.conf.d/99-storage-limits.conf <<EOF
[Journal]
SystemMaxUse=${MAINTENANCE_JOURNAL_MAX_SIZE}
SystemKeepFree=2G
MaxRetentionSec=${MAINTENANCE_JOURNAL_RETENTION}
EOF
  systemctl restart systemd-journald 2>/dev/null || true

  # Rotate Docker json-file logs if present.
  cat >/etc/logrotate.d/docker-container-json <<'EOF'
/var/lib/docker/containers/*/*-json.log {
    daily
    rotate 7
    maxsize 100M
    missingok
    notifempty
    compress
    delaycompress
    copytruncate
}
EOF

  # Rotate maintenance log.
  cat >/etc/logrotate.d/bootstrap-storage-maintenance <<'EOF'
/var/log/bootstrap-storage-maintenance.log {
    weekly
    rotate 8
    missingok
    notifempty
    compress
    delaycompress
    copytruncate
}
EOF

  # Idempotent maintenance script (daily + weekly mode).
  cat >/usr/local/sbin/bootstrap-storage-maintenance.sh <<EOF
#!/bin/bash
set -euo pipefail

MODE="\${1:-daily}"
TMP_RETENTION_DAYS="${MAINTENANCE_TMP_RETENTION_DAYS}"
JOURNAL_RETENTION="${MAINTENANCE_JOURNAL_RETENTION}"
JOURNAL_MAX_SIZE="${MAINTENANCE_JOURNAL_MAX_SIZE}"
DOCKER_PRUNE_UNTIL="${MAINTENANCE_DOCKER_PRUNE_UNTIL}"
DOCKER_BUILDER_KEEP_STORAGE="${MAINTENANCE_DOCKER_BUILDER_KEEP_STORAGE}"
PRESSURE_FREE_GB="${MAINTENANCE_PRESSURE_FREE_GB}"

export PATH=/usr/local/sbin:/usr/local/bin:/usr/sbin:/usr/bin:/sbin:/bin

if command -v systemd-tmpfiles >/dev/null 2>&1; then
  systemd-tmpfiles --clean 2>/dev/null || true
fi

find /tmp -xdev -mindepth 1 -mtime +"\$TMP_RETENTION_DAYS" -print0 2>/dev/null | xargs -0r rm -rf -- 2>/dev/null || true
find /var/tmp -xdev -mindepth 1 -mtime +"\$TMP_RETENTION_DAYS" -print0 2>/dev/null | xargs -0r rm -rf -- 2>/dev/null || true

if command -v journalctl >/dev/null 2>&1; then
  journalctl --vacuum-time="\$JOURNAL_RETENTION" >/dev/null 2>&1 || true
  journalctl --vacuum-size="\$JOURNAL_MAX_SIZE" >/dev/null 2>&1 || true
fi

apt-get autoclean -y -qq >/dev/null 2>&1 || true
apt-get autoremove -y -qq >/dev/null 2>&1 || true

if [ "\$MODE" = "pressure" ]; then
  FREE_GB=$(df --output=avail -BG / 2>/dev/null | tail -n1 | tr -dc '0-9')
  [ -z "\$FREE_GB" ] && FREE_GB=0

  if [ "\$FREE_GB" -ge "\$PRESSURE_FREE_GB" ]; then
    exit 0
  fi

  # EMERGENCY: Less than threshold - aggressive cleanup
  if command -v docker >/dev/null 2>&1; then
    echo "[PRESSURE] Only \${FREE_GB}GB free, triggering emergency cleanup..."

    # Level 1: Standard prune (removes unused)
    docker image prune -af --filter "until=\$DOCKER_PRUNE_UNTIL" >/dev/null 2>&1 || true
    docker container prune -f --filter "until=\$DOCKER_PRUNE_UNTIL" >/dev/null 2>&1 || true
    docker network prune -f >/dev/null 2>&1 || true
    docker builder prune -af --keep-storage "\$DOCKER_BUILDER_KEEP_STORAGE" >/dev/null 2>&1 || true
    docker buildx prune -af --keep-storage "\$DOCKER_BUILDER_KEEP_STORAGE" >/dev/null 2>&1 || true

    # Level 2: Remove backups of LARGEST images (emergency mode)
    if [ -f /usr/local/sbin/docker-image-retention.sh ]; then
      /usr/local/sbin/docker-image-retention.sh emergency 2>&1 | sed 's/^/[RETENTION] /' || true
    fi

    # Check if we freed enough space
    FREE_GB_AFTER=$(df --output=avail -BG / 2>/dev/null | tail -n1 | tr -dc '0-9')
    [ -z "\$FREE_GB_AFTER" ] && FREE_GB_AFTER=0

    # Level 3: NUCLEAR - Clear containerd snapshots if still critical (<10GB)
    if [ "\$FREE_GB_AFTER" -lt 10 ] && [ -d /var/lib/containerd ]; then
      echo "[EMERGENCY] Only \${FREE_GB_AFTER}GB after cleanup - purging containerd snapshots..."
      systemctl stop docker >/dev/null 2>&1 || true
      find /var/lib/containerd/io.containerd.snapshotter.v1.overlayfs/snapshots -mindepth 1 -maxdepth 1 -type d -mtime +7 -exec rm -rf {} + 2>/dev/null || true
      find /var/lib/containerd/io.containerd.content.v1.content/blobs -type f -mtime +7 -delete 2>/dev/null || true
      systemctl start docker >/dev/null 2>&1 || true
      echo "[EMERGENCY] Containerd cleanup complete - REBUILD YOUR IMAGES ASAP"
    fi
  fi
fi

if [ "\$MODE" = "weekly" ] && command -v docker >/dev/null 2>&1; then
  # Prune old images and enforce tag retention
  docker image prune -af --filter "until=\$DOCKER_PRUNE_UNTIL" >/dev/null 2>&1 || true
  docker container prune -f --filter "until=\$DOCKER_PRUNE_UNTIL" >/dev/null 2>&1 || true
  docker network prune -f --filter "until=\$DOCKER_PRUNE_UNTIL" >/dev/null 2>&1 || true
  docker volume prune -f >/dev/null 2>&1 || true
  docker builder prune -af --keep-storage "\$DOCKER_BUILDER_KEEP_STORAGE" >/dev/null 2>&1 || true
  docker buildx prune -af --keep-storage "\$DOCKER_BUILDER_KEEP_STORAGE" >/dev/null 2>&1 || true

  # Enforce tag retention (keep only latest + 1 backup per image)
  if [ -f /usr/local/sbin/docker-image-retention.sh ]; then
    /usr/local/sbin/docker-image-retention.sh >/dev/null 2>&1 || true
  fi
fi

# Daily: Run tag retention to prevent tag accumulation
if [ "\$MODE" = "daily" ] && command -v docker >/dev/null 2>&1; then
  if [ -f /usr/local/sbin/docker-image-retention.sh ]; then
    /usr/local/sbin/docker-image-retention.sh >/dev/null 2>&1 || true
  fi
fi

logrotate -f /etc/logrotate.d/docker-container-json >/dev/null 2>&1 || true
logrotate -f /etc/logrotate.d/bootstrap-storage-maintenance >/dev/null 2>&1 || true
EOF
  chmod 0755 /usr/local/sbin/bootstrap-storage-maintenance.sh

  # Install cron jobs idempotently.
  cat >/etc/cron.d/bootstrap-storage-maintenance <<EOF
SHELL=/bin/bash
PATH=/usr/local/sbin:/usr/local/bin:/usr/sbin:/usr/bin:/sbin:/bin

# Daily storage hygiene
${MAINTENANCE_DAILY_CRON} root /usr/local/sbin/bootstrap-storage-maintenance.sh daily >>/var/log/bootstrap-storage-maintenance.log 2>&1

# Weekly deep cleanup (no image prune)
${MAINTENANCE_WEEKLY_CRON} root /usr/local/sbin/bootstrap-storage-maintenance.sh weekly >>/var/log/bootstrap-storage-maintenance.log 2>&1

# Disk-pressure guard (extra safe pass when free space is below threshold)
${MAINTENANCE_PRESSURE_CRON} root /usr/local/sbin/bootstrap-storage-maintenance.sh pressure >>/var/log/bootstrap-storage-maintenance.log 2>&1
EOF
  chmod 0644 /etc/cron.d/bootstrap-storage-maintenance

  touch /var/log/bootstrap-storage-maintenance.log

  # Run daily cleanup once immediately.
  /usr/local/sbin/bootstrap-storage-maintenance.sh daily || true

  log_success "Automated storage maintenance configured"
fi

# ============================================================================
# CLEANUP
# ============================================================================

log_info "Cleaning up..."

# Remove old SSH config backups (older than 30 days)
find /etc/ssh -name "sshd_config.backup.*" -mtime +30 -delete 2>/dev/null || true

# Clean package manager cache
apt-get autoremove -y -qq
apt-get autoclean -y -qq

log_success "Cleanup complete"

# ============================================================================
# STACK INIT + COMPOSE DEPLOY
# ============================================================================

if [ "$ENABLE_STACK_DEPLOY" = "true" ] && [ "$ENABLE_DOCKER" = "true" ]; then
  log_info "Running stack init scripts and deploying Docker Compose..."

  if [ -d "$STACK_DIR" ]; then
    cd "$STACK_DIR"

    # ---- Generate .env from .env.example if missing ----
    if [ ! -f ".env" ] && [ -f ".env.example" ]; then
      log_info "  Generating .env from .env.example..."
      cp .env.example .env

      # Populate critical path variables with fully resolved values (no shell refs)
      sed -i "s|^ROOT_DIR=.*|ROOT_DIR=$STACK_DIR|" .env
      sed -i "s|^ROOT_PATH=.*|ROOT_PATH=$STACK_DIR|" .env
      sed -i "s|^REPO_ROOT=.*|REPO_ROOT=$STACK_DIR|" .env
      sed -i "s|^CERTS_DIR=.*|CERTS_DIR=$STACK_DIR/certs|" .env
      sed -i "s|^CERTS_PATH=.*|CERTS_PATH=$STACK_DIR/certs|" .env
      sed -i "s|^CONFIG_PATH=.*|CONFIG_PATH=$STACK_DIR/volumes|" .env
      sed -i "s|^DATA_DIR=.*|DATA_DIR=$STACK_DIR/data|" .env
      sed -i "s|^DATA_PATH=.*|DATA_PATH=$STACK_DIR/data|" .env
      sed -i "s|^SECRETS_DIR=.*|SECRETS_DIR=$STACK_DIR/secrets|" .env
      sed -i "s|^SECRETS_PATH=.*|SECRETS_PATH=$STACK_DIR/secrets|" .env
      sed -i "s|^CREDENTIALS_DIRECTORY=.*|CREDENTIALS_DIRECTORY=$STACK_DIR/secrets|" .env
      sed -i "s|^SRC_DIR=.*|SRC_DIR=$STACK_DIR/projects|" .env
      sed -i "s|^SRC_PATH=.*|SRC_PATH=$STACK_DIR/projects|" .env
      sed -i "s|^BACKUP_DIR=.*|BACKUP_DIR=$STACK_DIR/backup|" .env
      sed -i "s|^DOMAIN=.*|DOMAIN=${DOMAIN}|" .env
      sed -i "s|^TS_HOSTNAME=.*|TS_HOSTNAME=${HOSTNAME_SHORT}|" .env
      sed -i "s|^MAIN_USERNAME=.*|MAIN_USERNAME=${PRIMARY_USER}|" .env
      sed -i "s|^STACK_NAME=.*|STACK_NAME=${HOSTNAME_SHORT}|" .env

      # Generate a random SUDO_PASSWORD if empty
      if grep -qE '^SUDO_PASSWORD=$' .env; then
        GENERATED_SUDO_PASS=$(openssl rand -base64 24 2>/dev/null || head -c 32 /dev/urandom | base64 | tr -d '/+=' | head -c 24)
        sed -i "s|^SUDO_PASSWORD=.*|SUDO_PASSWORD=${GENERATED_SUDO_PASS}|" .env
        log_info "  Generated random SUDO_PASSWORD"
      fi

      # Detect external IP for compose
      EXTERNAL_IP_DETECTED=$(curl -fsSL --max-time 5 https://ifconfig.me 2>/dev/null || curl -fsSL --max-time 5 https://api.ipify.org 2>/dev/null || true)
      [ -n "$EXTERNAL_IP_DETECTED" ] && sed -i "s|^EXTERNAL_IP=.*|EXTERNAL_IP=${EXTERNAL_IP_DETECTED}|" .env

      # Set ACME email from domain
      grep -qE '^ACME_RESOLVER_EMAIL=$' .env && sed -i "s|^ACME_RESOLVER_EMAIL=.*|ACME_RESOLVER_EMAIL=admin@${DOMAIN}|" .env

      # Detect CPU count
      CPU_DETECTED=$(nproc 2>/dev/null || echo "4")
      sed -i "s|^CPU_COUNT=.*|CPU_COUNT=${CPU_DETECTED}|" .env

      # Ensure required directories exist (handle symlinks gracefully)
      for subdir in certs volumes data secrets projects backup; do
        target="$STACK_DIR/$subdir"
        if [ -L "$target" ]; then
          # Follow symlink and create the target directory
          link_target=$(readlink -f "$target" 2>/dev/null || readlink "$target")
          mkdir -p "$link_target" 2>/dev/null || true
        elif [ ! -d "$target" ]; then
          mkdir -p "$target" 2>/dev/null || true
        fi
      done

      chown "$STACK_USER":"$STACK_USER" .env 2>/dev/null || true
      log_success ".env generated with ROOT_DIR=$STACK_DIR"
    fi

    # ---- Generate .secrets from .secrets.example if missing ----
    if [ ! -f ".secrets" ] && [ -f ".secrets.example" ]; then
      log_info "  Generating .secrets from .secrets.example..."
      cp .secrets.example .secrets
      chown "$STACK_USER":"$STACK_USER" .secrets 2>/dev/null || true
      log_success ".secrets placeholder generated"
    fi

    # ---- Generate individual Docker secret files ----
    if [ -f "scripts/generate-secrets.sh" ]; then
      log_info "  Running generate-secrets.sh..."
      bash scripts/generate-secrets.sh --force 2>&1 || log_warn "generate-secrets.sh had warnings"
    else
      # Ensure secrets directory and minimal placeholder exist
      SECRETS_DIR_RESOLVED="${STACK_DIR}/secrets"
      mkdir -p "$SECRETS_DIR_RESOLVED"
      [ ! -f "$SECRETS_DIR_RESOLVED/signing_secret.txt" ] && openssl rand -hex 32 > "$SECRETS_DIR_RESOLVED/signing_secret.txt" 2>/dev/null || true
      chown -R "$STACK_USER":"$STACK_USER" "$SECRETS_DIR_RESOLVED" 2>/dev/null || true
    fi

    # ---- Merge .secrets values into .env for compose interpolation ----
    # Docker Compose reads .env but not .secrets; required vars must be in .env.
    if [ -f ".secrets" ] && [ -f ".env" ]; then
      log_info "  Merging .secrets values into .env..."
      merged=0
      while IFS='=' read -r key value; do
        # Skip comments and empty lines
        [[ "$key" =~ ^#.*$ ]] && continue
        [[ -z "$key" ]] && continue
        key=$(echo "$key" | xargs)
        [ -z "$key" ] && continue
        # Only merge if .env has this key but it's empty
        if grep -qE "^${key}=\$" .env 2>/dev/null && [ -n "$value" ]; then
          sed -i "s|^${key}=.*|${key}=${value}|" .env
          merged=$((merged + 1))
        fi
      done < <(if grep -qP '\r$' .secrets 2>/dev/null; then sed 's/\r$//' .secrets; else cat .secrets; fi)
      log_success "Merged $merged values from .secrets into .env"
    fi

    # ---- Seed any still-empty required compose vars with generated values ----
    # Use iterative docker compose config to discover and fix missing vars.
    if [ -f ".env" ] && command -v docker >/dev/null 2>&1; then
      log_info "  Seeding required variables via iterative compose validation..."
      seeded=0
      max_iterations=100
      iteration=0
      while [ "$iteration" -lt "$max_iterations" ]; do
        iteration=$((iteration + 1))
        config_err=$(docker compose -f "$STACK_COMPOSE_FILE" config 2>&1 >/dev/null) || true
        # Extract missing variable name from "required variable FOO is missing a value"
        missing_var=$(echo "$config_err" | grep -oP 'required variable \K[A-Z_0-9]+(?= is missing)' 2>/dev/null | head -1) || true
        [ -z "$missing_var" ] && break

        gen_val=$(openssl rand -hex 16 2>/dev/null || echo "placeholder_$(date +%s)")
        if grep -q "^${missing_var}=" .env 2>/dev/null; then
          sed -i "s|^${missing_var}=.*|${missing_var}=${gen_val}|" .env
        else
          echo "${missing_var}=${gen_val}" >> .env
        fi
        seeded=$((seeded + 1))
      done
      [ "$seeded" -gt 0 ] && log_success "Seeded $seeded required variables with generated values"
      [ "$iteration" -ge "$max_iterations" ] && log_warn "Hit max iterations ($max_iterations) seeding required vars"
    fi

    # ---- CrowdSec preflight (must happen before crowdsec-init deploy) ----
    # Ensure parent folders and files referenced by compose configs exist.
    log_info "  Running CrowdSec preflight path preparation..."
    CONFIG_PATH_RESOLVED="$STACK_DIR/volumes"
    if [ -f ".env" ]; then
      CONFIG_PATH_RESOLVED="$({
        set -a
        source ./.env >/dev/null 2>&1 || true
        set +a
        echo "${CONFIG_PATH:-$STACK_DIR/volumes}"
      })"
    fi

    # Resolve relative CONFIG_PATH against stack dir.
    case "$CONFIG_PATH_RESOLVED" in
      /*) ;;
      *) CONFIG_PATH_RESOLVED="$STACK_DIR/${CONFIG_PATH_RESOLVED#./}" ;;
    esac

    CROWDSEC_LOG_DIR="$CONFIG_PATH_RESOLVED/traefik/crowdsec/var/log"
    mkdir -p "$CROWDSEC_LOG_DIR"
    touch "$CROWDSEC_LOG_DIR/auth.log" "$CROWDSEC_LOG_DIR/syslog"

    # Also ensure CrowdSec persistent dirs exist.
    mkdir -p \
      "$CONFIG_PATH_RESOLVED/traefik/crowdsec/data" \
      "$CONFIG_PATH_RESOLVED/traefik/crowdsec/etc/crowdsec" \
      "$CONFIG_PATH_RESOLVED/traefik/crowdsec/plugins"

    chown "$STACK_USER":"$STACK_USER" "$CROWDSEC_LOG_DIR/auth.log" "$CROWDSEC_LOG_DIR/syslog" 2>/dev/null || true
    log_success "CrowdSec preflight paths ensured at $CROWDSEC_LOG_DIR"

    # ---- Run init scripts ----
    for init_script in $STACK_INIT_SCRIPTS; do
      if [ -f "$init_script" ]; then
        log_info "  Running init script: $init_script"
        if [ -x "$init_script" ]; then
          "$init_script" || log_warn "Init script failed: $init_script"
        else
          bash "$init_script" || log_warn "Init script failed: $init_script"
        fi
      else
        log_warn "Init script not found: $init_script (skipping)"
      fi
    done

    # ---- Pre-create bind-mount paths for Docker configs and secrets ----
    log_info "  Pre-creating bind-mount paths for configs and secrets..."
    # Configs: create parent dirs and touch missing files so Docker can bind-mount them
    docker compose -f "$STACK_COMPOSE_FILE" config --format json 2>/dev/null \
      | jq -r '.configs // {} | to_entries[] | .value.file // empty' 2>/dev/null \
      | while IFS= read -r cfg_file; do
          if [ -n "$cfg_file" ] && [ ! -f "$cfg_file" ]; then
            mkdir -p "$(dirname "$cfg_file")" && touch "$cfg_file"
            log_info "    Created missing config file: $cfg_file"
          fi
        done || true
    # Secrets: create parent dirs and touch missing secret files
    docker compose -f "$STACK_COMPOSE_FILE" config --format json 2>/dev/null \
      | jq -r '.secrets // {} | to_entries[] | .value.file // empty' 2>/dev/null \
      | while IFS= read -r sec_file; do
          if [ -n "$sec_file" ] && [ ! -f "$sec_file" ]; then
            mkdir -p "$(dirname "$sec_file")" && touch "$sec_file"
            log_info "    Created missing secret file: $sec_file"
          fi
        done || true

    # ---- Docker Compose deploy ----
    log_info "  Validating compose file..."
    if docker compose -f "$STACK_COMPOSE_FILE" config >/dev/null 2>&1; then
      # Skip explicit pull to rely on cached images from earlier runs (rate limits prevent full pull)
      # log_info "  Compose validation passed; pulling images (ignoring failures for rate limits)..."
      # docker compose -f "$STACK_COMPOSE_FILE" pull --ignore-pull-failures --quiet 2>&1 | grep -v 'Interrupted' || true

      # Build explicit service list from active compose config and remove excluded services.
      mapfile -t compose_services < <(docker compose -f "$STACK_COMPOSE_FILE" config --services 2>/dev/null || true)
      deploy_services=()
      for svc in "${compose_services[@]}"; do
        skip_service="false"
        for excluded in $STACK_EXCLUDE_SERVICES; do
          if [ "$svc" = "$excluded" ]; then
            skip_service="true"
            break
          fi
        done

        if [ "$skip_service" = "true" ]; then
          log_info "  Skipping excluded service: $svc"
        else
          #Check if image exists locally before adding to deploy list
          svc_image=$(docker compose -f "$STACK_COMPOSE_FILE" config --format json 2>/dev/null | jq -r ".services[\"$svc\"].image // empty" 2>/dev/null | head -1)
          if [ -n "$svc_image" ] && docker image inspect "$svc_image" >/dev/null 2>&1; then
            deploy_services+=("$svc")
          elif [ -n "$svc" ]; then
            log_info "  Skipping service without local image: $svc${svc_image:+ (needs $svc_image)}"
          fi
        fi
      done

      log_info "  Starting available services..."
      if [ "${#deploy_services[@]}" -eq 0 ]; then
        log_warn "No deployable services found with local images; skipping docker compose up"
      else
        log_info "  Deploying ${#deploy_services[@]} services with available images"
        # Use || true to allow partial success — services with missing dependencies will be skipped
        docker compose -f "$STACK_COMPOSE_FILE" up -d $STACK_COMPOSE_UP_ARGS "${deploy_services[@]}" 2>&1 | tee -a /tmp/compose-up.log || true
      fi

      running_count=$(docker ps -q | wc -l)
      log_success "Docker Compose deploy completed: $running_count containers running"
    else
      log_warn "Compose file validation failed: $STACK_DIR/$STACK_COMPOSE_FILE"
      log_warn "  Run 'docker compose -f $STACK_COMPOSE_FILE config' manually to debug"
    fi
  else
    log_warn "Stack directory not found: $STACK_DIR"
  fi
fi

# ============================================================================
# FINAL STATUS
# ============================================================================

log_info ""
log_info "========================================"
log_info "  VPS Bootstrap Complete!"
log_info "========================================"
log_info "  Hostname: ${HOSTNAME_SHORT}"
log_info "  FQDN: ${FQDN}"
[ "$ENABLE_DOCKER" = "true" ] && command -v docker >/dev/null 2>&1 && log_info "  Docker: $(docker --version 2>/dev/null)"
[ "$ENABLE_TAILSCALE" = "true" ] && command -v tailscale >/dev/null 2>&1 && log_info "  Tailscale: Connected"
[ "$ENABLE_NOMAD" = "true" ] && command -v nomad >/dev/null 2>&1 && log_info "  Nomad: $(nomad -v 2>/dev/null)"
log_info "========================================"
log_info ""

echo "VPS Server '${HOSTNAME_SHORT}' is ready!" | tee /var/log/cloud-init-complete.log
