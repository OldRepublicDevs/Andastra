meta:
  id: mdl_ascii
  title: BioWare MDL ASCII Format
  license: MIT
  endian: le
  file-extension: mdl.ascii
  encoding: UTF-8
  ks-version: 0.11
doc: |
  MDL ASCII format is a human-readable ASCII text representation of MDL (Model) binary files.
  Used by modding tools for easier editing than binary MDL format.

  The ASCII format represents the model structure using plain text with keyword-based syntax.
  Lines are parsed sequentially, with keywords indicating sections, nodes, properties, and data arrays.

  Reference: vendor/PyKotor/wiki/MDL-MDX-File-Format.md - ASCII MDL Format section
  Reference: vendor/mdlops/MDLOpsM.pm:3916-4698 - readasciimdl function implementation

doc-ref: https://github.com/th3w1zard1/PyKotor/wiki/MDL-MDX-File-Format.md#ascii-mdl-format

seq:
  - id: lines
    type: ascii_line
    repeat: eos

types:
  ascii_line:
    doc: Single line in ASCII MDL file
    seq:
      - id: content
        type: str
        size-eos: true
        encoding: UTF-8
        terminator: 10
        consume: false

  model_header:
    doc: Model header section keywords
    seq:
      - id: newmodel
        type: str
        doc: "newmodel <model_name> - Declares the model name"
      - id: setsupermodel
        type: str
        doc: "setsupermodel <model_name> <supermodel_name> - Sets parent model for inheritance"
      - id: classification
        type: str
        doc: "classification <type> - Model classification (Other, Effect, Tile, Character, Door, Lightsaber, Placeable, Flyer)"
      - id: classification_unk1
        type: str
        doc: "classification_unk1 <value> - Unknown classification byte"
      - id: ignorefog
        type: str
        doc: "ignorefog <0_or_1> - Whether model is affected by fog (0=affected, 1=ignored)"
      - id: setanimationscale
        type: str
        doc: "setanimationscale <scale_factor> - Animation scale factor (typically 0.971)"
      - id: compress_quaternions
        type: str
        doc: "compress_quaternions <0_or_1> - Enable quaternion compression"
      - id: bumpmapped_texture
        type: str
        doc: "bumpmapped_texture <texture_name> - Declares texture as bump-mapped (requires tangent space)"
      - id: headlink
        type: str
        doc: "headlink <node_name> - Model-wide headfix link"

  geometry_section:
    doc: Geometry section keywords
    seq:
      - id: beginmodelgeom
        type: str
        doc: "beginmodelgeom <model_name> - Start of model geometry section"
      - id: endmodelgeom
        type: str
        doc: "endmodelgeom - End of model geometry section"
      - id: bmin
        type: str
        doc: "bmin <x> <y> <z> - Minimum bounding box coordinates"
      - id: bmax
        type: str
        doc: "bmax <x> <y> <z> - Maximum bounding box coordinates"
      - id: radius
        type: str
        doc: "radius <value> - Model bounding sphere radius"

  node_definition:
    doc: Node definition keywords
    seq:
      - id: node
        type: str
        doc: "node <node_type> <node_name> - Start of node definition. Types: dummy, light, emitter, reference, trimesh, skinmesh, animmesh, danglymesh, aabb, lightsaber"
      - id: endnode
        type: str
        doc: "endnode - End of node definition"
      - id: parent
        type: str
        doc: "parent <parent_name> - Parent node name (null if root)"
      - id: position
        type: str
        doc: "position <x> <y> <z> - Node position in local space"
      - id: orientation
        type: str
        doc: "orientation <x> <y> <z> <w> - Node orientation as quaternion (W, X, Y, Z) or angle-axis (X, Y, Z, angle) in ASCII"
      - id: scale
        type: str
        doc: "scale <x> <y> <z> - Node scale factors (or single value for uniform scaling)"

  mesh_properties:
    doc: Mesh node properties
    seq:
      - id: bitmap
        type: str
        doc: "bitmap <texture_name> - Primary diffuse texture name"
      - id: bitmap2
        type: str
        doc: "bitmap2 <texture_name> - Secondary texture name (often lightmap)"
      - id: texture0
        type: str
        doc: "texture0 <texture_name> - Primary texture name (alternative to bitmap)"
      - id: texture1
        type: str
        doc: "texture1 <texture_name> - Secondary texture name (alternative to bitmap2)"
      - id: lightmap
        type: str
        doc: "lightmap <texture_name> - Lightmap texture (magnusll export compatibility, sets lightmapped flag)"
      - id: diffuse
        type: str
        doc: "diffuse <r> <g> <b> - Material diffuse color (RGB, range 0.0-1.0)"
      - id: ambient
        type: str
        doc: "ambient <r> <g> <b> - Material ambient color (RGB, range 0.0-1.0)"
      - id: specular
        type: str
        doc: "specular <r> <g> <b> - Material specular color (not used in binary, for reference only)"
      - id: shininess
        type: str
        doc: "shininess <value> - Material shininess (not used in binary, for reference only)"
      - id: render
        type: str
        doc: "render <0_or_1> - Whether mesh is renderable (1=visible, 0=hidden)"
      - id: shadow
        type: str
        doc: "shadow <0_or_1> - Whether mesh casts shadows (1=casts, 0=no shadows)"
      - id: lightmapped
        type: str
        doc: "lightmapped <0_or_1> - Whether mesh uses lightmap (1=uses, 0=no lightmap)"
      - id: rotatetexture
        type: str
        doc: "rotatetexture <0_or_1> - Whether texture should rotate (1=rotate, 0=static)"
      - id: m_b_is_background_geometry
        type: str
        doc: "m_bIsBackgroundGeometry <0_or_1> - Whether mesh is background geometry"
      - id: tangentspace
        type: str
        doc: "tangentspace <0_or_1> - Whether mesh uses tangent space (for bump mapping)"
      - id: beaming
        type: str
        doc: "beaming <0_or_1> - Whether beaming effect is enabled"
      - id: transparencyhint
        type: str
        doc: "transparencyhint <value> - Transparency rendering mode"
      - id: dirt_enabled
        type: str
        doc: "dirt_enabled <0_or_1> - KOTOR 2 dirt effect enabled"
      - id: dirt_texture
        type: str
        doc: "dirt_texture <texture_name> - KOTOR 2 dirt texture name"
      - id: dirt_worldspace
        type: str
        doc: "dirt_worldspace <0_or_1> - KOTOR 2 dirt worldspace flag"
      - id: hologram_donotdraw
        type: str
        doc: "hologram_donotdraw <0_or_1> - KOTOR 2 hologram do not draw flag"
      - id: animateuv
        type: str
        doc: "animateuv <0_or_1> - Whether UV animation is enabled"
      - id: uvdirectionx
        type: str
        doc: "uvdirectionx <value> - UV animation direction X component"
      - id: uvdirectiony
        type: str
        doc: "uvdirectiony <value> - UV animation direction Y component"
      - id: uvjitter
        type: str
        doc: "uvjitter <value> - UV animation jitter amount"
      - id: uvjitterspeed
        type: str
        doc: "uvjitterspeed <value> - UV animation jitter speed"
      - id: bmin
        type: str
        doc: "bmin <x> <y> <z> - Mesh bounding box minimum (within node)"
      - id: bmax
        type: str
        doc: "bmax <x> <y> <z> - Mesh bounding box maximum (within node)"
      - id: radius
        type: str
        doc: "radius <value> - Mesh bounding sphere radius"
      - id: average
        type: str
        doc: "average <x> <y> <z> - Average vertex position (centroid)"
      - id: inv_count
        type: str
        doc: "inv_count <value> [<value2>] - Inverted mesh sequence counter"

  light_properties:
    doc: Light node properties
    seq:
      - id: flareradius
        type: str
        doc: "flareradius <value> - Radius of lens flare effect"
      - id: flarepositions
        type: str
        doc: "flarepositions <count> - Start flare positions array (count floats, 0.0-1.0 along light ray)"
      - id: flaresizes
        type: str
        doc: "flaresizes <count> - Start flare sizes array (count floats)"
      - id: flarecolorshifts
        type: str
        doc: "flarecolorshifts <count> - Start flare color shifts array (count RGB floats)"
      - id: texturenames
        type: str
        doc: "texturenames <count> - Start flare texture names array (count strings)"
      - id: ambientonly
        type: str
        doc: "ambientonly <0_or_1> - Whether light only affects ambient (1=ambient only, 0=full lighting)"
      - id: ndynamictype
        type: str
        doc: "ndynamictype <value> - Type of dynamic lighting behavior"
      - id: affectdynamic
        type: str
        doc: "affectdynamic <0_or_1> - Whether light affects dynamic objects (1=affects, 0=static only)"
      - id: flare
        type: str
        doc: "flare <0_or_1> - Whether lens flare effect is enabled (1=enabled, 0=disabled)"
      - id: lightpriority
        type: str
        doc: "lightpriority <value> - Rendering priority for light culling/sorting"
      - id: fadinglight
        type: str
        doc: "fadinglight <0_or_1> - Whether light intensity fades with distance (1=fades, 0=constant)"

  emitter_properties:
    doc: Emitter node properties
    seq:
      - id: deadspace
        type: str
        doc: "deadspace <value> - Minimum distance from emitter before particles become visible"
      - id: blast_radius
        type: str
        doc: "blastRadius <value> - Radius of explosive/blast particle effects"
      - id: blast_length
        type: str
        doc: "blastLength <value> - Length/duration of blast effects"
      - id: num_branches
        type: str
        doc: "numBranches <value> - Number of branching paths for particle trails"
      - id: controlptsmoothing
        type: str
        doc: "controlptsmoothing <value> - Smoothing factor for particle path control points"
      - id: xgrid
        type: str
        doc: "xgrid <value> - Grid subdivisions along X axis for particle spawning"
      - id: ygrid
        type: str
        doc: "ygrid <value> - Grid subdivisions along Y axis for particle spawning"
      - id: spawntype
        type: str
        doc: "spawntype <value> - Particle spawn type"
      - id: update
        type: str
        doc: "update <script_name> - Update behavior script name (e.g., single, fountain)"
      - id: render
        type: str
        doc: "render <script_name> - Render mode script name (e.g., normal, billboard_to_local_z)"
      - id: blend
        type: str
        doc: "blend <script_name> - Blend mode script name (e.g., normal, lighten)"
      - id: texture
        type: str
        doc: "texture <texture_name> - Particle texture name"
      - id: chunkname
        type: str
        doc: "chunkname <chunk_name> - Associated model chunk name"
      - id: twosidedtex
        type: str
        doc: "twosidedtex <0_or_1> - Whether texture should render two-sided (1=two-sided, 0=single-sided)"
      - id: loop
        type: str
        doc: "loop <0_or_1> - Whether particle system loops (1=loops, 0=single playback)"
      - id: renderorder
        type: str
        doc: "renderorder <value> - Rendering priority/order for particle sorting"
      - id: m_b_frame_blending
        type: str
        doc: "m_bFrameBlending <0_or_1> - Whether frame blending is enabled (1=enabled, 0=disabled)"
      - id: m_s_depth_texture_name
        type: str
        doc: "m_sDepthTextureName <texture_name> - Depth/softparticle texture name"

  emitter_flags:
    doc: Emitter behavior flags
    seq:
      - id: p2p
        type: str
        doc: "p2p <0_or_1> - Point-to-point flag (bit 0x0001)"
      - id: p2p_sel
        type: str
        doc: "p2p_sel <0_or_1> - Point-to-point selection flag (bit 0x0002)"
      - id: affected_by_wind
        type: str
        doc: "affectedByWind <0_or_1> - Affected by wind flag (bit 0x0004)"
      - id: m_is_tinted
        type: str
        doc: "m_isTinted <0_or_1> - Is tinted flag (bit 0x0008)"
      - id: bounce
        type: str
        doc: "bounce <0_or_1> - Bounce flag (bit 0x0010)"
      - id: random
        type: str
        doc: "random <0_or_1> - Random flag (bit 0x0020)"
      - id: inherit
        type: str
        doc: "inherit <0_or_1> - Inherit flag (bit 0x0040)"
      - id: inheritvel
        type: str
        doc: "inheritvel <0_or_1> - Inherit velocity flag (bit 0x0080)"
      - id: inherit_local
        type: str
        doc: "inherit_local <0_or_1> - Inherit local flag (bit 0x0100)"
      - id: splat
        type: str
        doc: "splat <0_or_1> - Splat flag (bit 0x0200)"
      - id: inherit_part
        type: str
        doc: "inherit_part <0_or_1> - Inherit part flag (bit 0x0400)"
      - id: depth_texture
        type: str
        doc: "depth_texture <0_or_1> - Depth texture flag (bit 0x0800)"
      - id: emitterflag13
        type: str
        doc: "emitterflag13 <0_or_1> - Emitter flag 13 (bit 0x1000)"

  reference_properties:
    doc: Reference node properties
    seq:
      - id: refmodel
        type: str
        doc: "refmodel <model_resref> - Referenced model resource name without extension"
      - id: reattachable
        type: str
        doc: "reattachable <0_or_1> - Whether model can be detached and reattached dynamically (1=reattachable, 0=permanent)"

  danglymesh_properties:
    doc: Danglymesh node properties
    seq:
      - id: displacement
        type: str
        doc: "displacement <value> - Maximum displacement distance for physics simulation"
      - id: tightness
        type: str
        doc: "tightness <value> - Tightness/stiffness of spring simulation (0.0-1.0)"
      - id: period
        type: str
        doc: "period <value> - Oscillation period in seconds"

  data_arrays:
    doc: Data array keywords
    seq:
      - id: verts
        type: str
        doc: "verts <count> - Start vertex positions array (count vertices, 3 floats each: X, Y, Z)"
      - id: faces
        type: str
        doc: "faces <count> - Start faces array (count faces, format: normal_x normal_y normal_z plane_coeff mat_id adj1 adj2 adj3 v1 v2 v3 [t1 t2 t3])"
      - id: tverts
        type: str
        doc: "tverts <count> - Start primary texture coordinates array (count UVs, 2 floats each: U, V)"
      - id: tverts1
        type: str
        doc: "tverts1 <count> - Start secondary texture coordinates array (count UVs, 2 floats each: U, V)"
      - id: lightmaptverts
        type: str
        doc: "lightmaptverts <count> - Start lightmap texture coordinates (magnusll export compatibility, same as tverts1)"
      - id: tverts2
        type: str
        doc: "tverts2 <count> - Start tertiary texture coordinates array (count UVs, 2 floats each: U, V)"
      - id: tverts3
        type: str
        doc: "tverts3 <count> - Start quaternary texture coordinates array (count UVs, 2 floats each: U, V)"
      - id: texindices1
        type: str
        doc: "texindices1 <count> - Start texture indices array for 2nd texture (count face indices, 3 indices per face)"
      - id: texindices2
        type: str
        doc: "texindices2 <count> - Start texture indices array for 3rd texture (count face indices, 3 indices per face)"
      - id: texindices3
        type: str
        doc: "texindices3 <count> - Start texture indices array for 4th texture (count face indices, 3 indices per face)"
      - id: colors
        type: str
        doc: "colors <count> - Start vertex colors array (count colors, 3 floats each: R, G, B)"
      - id: colorindices
        type: str
        doc: "colorindices <count> - Start vertex color indices array (count face indices, 3 indices per face)"
      - id: weights
        type: str
        doc: "weights <count> - Start bone weights array (count weights, format: bone1 weight1 [bone2 weight2 [bone3 weight3 [bone4 weight4]]])"
      - id: constraints
        type: str
        doc: "constraints <count> - Start vertex constraints array for danglymesh (count floats, one per vertex)"
      - id: aabb
        type: str
        doc: "aabb [min_x min_y min_z max_x max_y max_z leaf_part] - AABB tree node (can be inline or multi-line)"
      - id: saber_verts
        type: str
        doc: "saber_verts <count> - Start lightsaber vertex positions array (count vertices, 3 floats each: X, Y, Z)"
      - id: saber_norms
        type: str
        doc: "saber_norms <count> - Start lightsaber vertex normals array (count normals, 3 floats each: X, Y, Z)"
      - id: name
        type: str
        doc: "name <node_name> - MDLedit-style name entry for walkmesh nodes (fakenodes)"

  animation_section:
    doc: Animation section keywords
    seq:
      - id: newanim
        type: str
        doc: "newanim <animation_name> <model_name> - Start of animation definition"
      - id: doneanim
        type: str
        doc: "doneanim <animation_name> <model_name> - End of animation definition"
      - id: length
        type: str
        doc: "length <duration> - Animation duration in seconds"
      - id: transtime
        type: str
        doc: "transtime <transition_time> - Transition/blend time to this animation in seconds"
      - id: animroot
        type: str
        doc: "animroot <root_node_name> - Root node name for animation"
      - id: event
        type: str
        doc: "event <time> <event_name> - Animation event (triggers at specified time)"
      - id: eventlist
        type: str
        doc: "eventlist - Start of animation events list"
      - id: endlist
        type: str
        doc: "endlist - End of list (controllers, events, etc.)"

  controller_single:
    doc: Single (constant) controller format
    seq:
      - id: controller_name
        type: str
        doc: Controller name (position, orientation, scale, color, radius, etc.)
      - id: values
        type: str
        doc: Space-separated controller values (number depends on controller type)

  controller_keyed:
    doc: Keyed (animated) controller format
    seq:
      - id: controller_name
        type: str
        doc: Controller name followed by 'key' (e.g., positionkey, orientationkey)
      - id: keyframes
        type: controller_keyframe
        repeat: eos
        doc: Keyframe entries until endlist keyword

  controller_bezier:
    doc: Bezier (smooth animated) controller format
    seq:
      - id: controller_name
        type: str
        doc: Controller name followed by 'bezierkey' (e.g., positionbezierkey, orientationbezierkey)
      - id: keyframes
        type: controller_bezier_keyframe
        repeat: eos
        doc: Keyframe entries until endlist keyword

  controller_keyframe:
    doc: Single keyframe in keyed controller
    seq:
      - id: time
        type: str
        size-eos: true
        encoding: UTF-8
        doc: Time value (float)
      - id: values
        type: str
        size-eos: true
        encoding: UTF-8
        doc: Space-separated property values (number depends on controller type and column count)

  controller_bezier_keyframe:
    doc: Single keyframe in Bezier controller (stores value + in_tangent + out_tangent per column)
    seq:
      - id: time
        type: str
        size-eos: true
        encoding: UTF-8
        doc: Time value (float)
      - id: value_data
        type: str
        size-eos: true
        encoding: UTF-8
        doc: "Space-separated values (3 times column_count floats: value, in_tangent, out_tangent for each column)"

enums:
  node_type:
    1: dummy
    3: light
    5: emitter
    17: reference
    33: trimesh
    97: skinmesh
    161: animmesh
    289: danglymesh
    545: aabb
    2081: lightsaber

  model_classification:
    0: other
    1: effect
    2: tile
    4: character
    8: door
    16: lightsaber
    32: placeable
    64: flyer

  controller_type_common:
    8: position
    20: orientation
    36: scale
    132: alpha

  controller_type_light:
    76: color
    88: radius
    96: shadowradius
    100: verticaldisplacement
    140: multiplier

  controller_type_emitter:
    80: alpha_end
    84: alpha_start
    88: birthrate
    92: bounce_co
    96: combinetime
    100: drag
    104: fps
    108: frame_end
    112: frame_start
    116: grav
    120: life_exp
    124: mass
    128: p2p_bezier2
    132: p2p_bezier3
    136: particle_rot
    140: randvel
    144: size_start
    148: size_end
    152: size_start_y
    156: size_end_y
    160: spread
    164: threshold
    168: velocity
    172: xsize
    176: ysize
    180: blurlength
    184: lightning_delay
    188: lightning_radius
    192: lightning_scale
    196: lightning_sub_div
    200: lightningzigzag
    216: alpha_mid
    220: percent_start
    224: percent_mid
    228: percent_end
    232: size_mid
    236: size_mid_y
    240: m_f_random_birth_rate
    252: targetsize
    256: numcontrolpts
    260: controlptradius
    264: controlptdelay
    268: tangentspread
    272: tangentlength
    284: color_mid
    380: color_end
    392: color_start
    502: detonate

  controller_type_mesh:
    100: selfillumcolor
