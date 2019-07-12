@echo off
meson build -Dv-hacd:ocl_root="%OCL_ROOT%" & pushd build & ninja & popd
