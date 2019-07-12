project('v-hacd', 'cpp')
cppc = meson.get_compiler('cpp')
add_project_arguments('-fpermissive', language: 'cpp')

ocl_root = get_option('ocl_root')
ocl_lib  = ocl_root == '' ? [] : [join_paths(ocl_root, 'lib', target_machine.cpu_family())]
ocl_inc  = ocl_root == '' ? [] : [join_paths(ocl_root, 'include')]

opencl = get_option('no_opencl') ? dependency('', required: false) : cppc.find_library('OpenCL',
    required: false,
    dirs:     ['/usr/lib'] + ocl_lib
)

if opencl.found()
    add_project_arguments('-DOPENCL_FOUND=1', language: 'cpp')
endif

opencl_dep = declare_dependency(
    dependencies:        opencl,
    include_directories: include_directories(ocl_inc)
)

openmp_dep = get_option('no_openmp') ? dependency('', required: false) : dependency('openmp',
    required: false
)

vhacd = both_libraries('vhacd',
    files(
        'source/btAlignedAllocator.cpp',
        'source/btConvexHullComputer.cpp',
        'source/FloatMath.cpp',
        'source/VHACD.cpp',
        'source/VHACD-ASYNC.cpp',
        'source/vhacdICHull.cpp',
        'source/vhacdManifoldMesh.cpp',
        'source/vhacdMesh.cpp',
        'source/vhacdRaycastMesh.cpp',
        'source/vhacdVolume.cpp'
    ),

    include_directories: include_directories(
        'include',
        'include/public'
    ),

    dependencies: [opencl_dep, openmp_dep]
)

vhacd_dep = declare_dependency(
    include_directories: include_directories('include/public'),
    link_with:           vhacd
)

vhacd_dep_static = declare_dependency(
    include_directories: include_directories('include/public'),
    link_with:           vhacd.get_static_lib()
)
