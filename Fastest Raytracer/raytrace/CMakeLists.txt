file(GLOB_RECURSE SOURCE_FILES
        "*.h"
        "*.cpp"
        "*.hpp"
        )

# OpenGL
find_package(OpenGL REQUIRED)
include_directories(${OpenGL_INCLUDE_DIRS})
link_directories(${OpenGL_LIBRARY_DIRS})
add_definitions(${OpenGL_DEFINITIONS})
if (NOT OPENGL_FOUND)
    message(ERROR "OPENGL not found!")
endif (NOT OPENGL_FOUND)

# GLUT
find_package(GLUT REQUIRED)
include_directories(${GLUT_INCLUDE_DIR})
link_directories(${GLUT_LIBRARY_DIRS})
if (NOT GLUT_FOUND)
    message(ERROR "GLUT not found!")
endif (NOT GLUT_FOUND)

# GLM
find_package(GLM REQUIRED)
include_directories(${GLM_INCLUDE_DIRS})
link_directories(${GLM_LIBRARY_DIRS})
add_definitions(${GLM_DEFINITIONS})
if (NOT GLM_FOUND)
    message(Error "GLM not found")
endif (NOT GLM_FOUND)

add_library(raytrace ${SOURCE_FILES})
if (DEFINED GLEW_LIB_NAME)
    target_link_libraries(raytrace ${OPENGL_LIBRARIES} ${GLUT_LIBRARIES} ${GLEW_LIBRARIES} ${GLM_LIBRARIES} ${GLEW_LIB_NAME})
else ()
    target_link_libraries(raytrace ${OPENGL_LIBRARIES} ${GLUT_LIBRARIES} ${GLEW_LIBRARIES} ${GLM_LIBRARIES} glew32)
endif ()

