cmake_minimum_required(VERSION 2.8.3)
project(spot_mil)

## Find catkin and any catkin packages
find_package(catkin REQUIRED COMPONENTS roscpp roslib rospy std_msgs genmsg spot_ros_msgs cv_bridge)

#find_package(Boost REQUIRED COMPONENTS thread)

find_package(PythonLibs REQUIRED)
## Declare a catkin package
catkin_package()

include_directories(include ${catkin_INCLUDE_DIRS})

catkin_python_setup()
