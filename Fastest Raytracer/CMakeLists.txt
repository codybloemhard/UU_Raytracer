cmake_minimum_required(VERSION 3.9)
project(raytrace)

set(CMAKE_MODULE_PATH ${CMAKE_CURRENT_SOURCE_DIR}/cmake)

set(CMAKE_CXX_STANDARD 11)

enable_testing()

add_subdirectory(main)
add_subdirectory(raytrace)
add_subdirectory(test)
