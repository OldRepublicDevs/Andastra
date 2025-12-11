$file = "src/CSharpKOTOR/Common/Script/ScriptDefs.cs"
$content = Get-Content $file -Raw

# Map of constant names to their values
$constants = @{
    'TALKVOLUME_TALK' = '0'
    'OBJECT_TYPE_CREATURE' = '1'
    'OBJECT_TYPE_ALL' = '32767'
    'OBJECT_TYPE_INVALID' = '32767'
    'DAMAGE_TYPE_UNIVERSAL' = '8'
    'DAMAGE_POWER_NORMAL' = '0'
    'SAVING_THROW_TYPE_NONE' = '0'
    'SAVING_THROW_TYPE_ALL' = '0'
    'AC_DODGE_BONUS' = '0'
    'AC_VS_DAMAGE_TYPE_ALL' = '8199'
    'ATTACK_BONUS_MISC' = '0'
    'PROJECTILE_PATH_TYPE_DEFAULT' = '0'
    'CONVERSATION_TYPE_CINEMATIC' = '0'
    'PERSISTENT_ZONE_ACTIVE' = '0'
    'ALIGNMENT_ALL' = '0'
    'FORCE_POWER_ALL_FORCE_POWERS' = '0'
}

foreach ($key in $constants.Keys) {
    $value = $constants[$key]
    # Replace standalone constant names (word boundaries)
    $content = $content -replace "\b$key\b", $value
}

Set-Content $file $content
Write-Host "Fixed ScriptDefs constants"

