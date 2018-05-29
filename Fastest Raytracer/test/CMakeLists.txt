if(DEFINED GMOCK_ROOT)
    add_subdirectory(${GMOCK_ROOT} gmock)
else()
    add_subdirectory($ENV{GMOCK_ROOT} gmock)
endif()

include_directories(${gtest_SOURCE_DIR}/include ${gtest_SOURCE_DIR})
include_directories(${gmock_SOURCE_DIR}/include ${gmock_SOURCE_DIR})

add_subdirectory(test_main)
