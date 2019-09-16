project('v-hacd-wrapper', 'cpp')

vhacd_prj = subproject('v-hacd')
vhacd_dep = vhacd_prj.get_variable('vhacd_dep_static')

shared_library('VHACD',
    files(
        'source/main.cpp',
        'source/context.cpp'
    ),

    dependencies: vhacd_dep,
    name_prefix:  'lib'
)
