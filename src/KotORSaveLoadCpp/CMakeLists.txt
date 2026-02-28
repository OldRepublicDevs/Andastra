cmake_minimum_required(VERSION 3.14)
project(KotORSaveLoadCpp LANGUAGES CXX)

set(CMAKE_CXX_STANDARD 17)

add_executable(kotor_saveload_test
  main.cpp
  KotORSaveLoad.cpp
  GffMemoryBackend.cpp
)
target_include_directories(kotor_saveload_test PRIVATE ${CMAKE_CURRENT_SOURCE_DIR})
