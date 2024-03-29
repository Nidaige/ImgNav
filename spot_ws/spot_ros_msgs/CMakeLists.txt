cmake_minimum_required(VERSION 2.8.3)
project(spot_ros_msgs)

find_package(catkin REQUIRED COMPONENTS
  std_msgs
  geometry_msgs
  sensor_msgs
  diagnostic_msgs
  message_generation
)

## Generate messages in the 'msg' folder
add_message_files(
  FILES
  KinematicState.msg
  ImageCapture.msg
  PowerState.msg
  CommsState.msg
  SystemFault.msg
  SystemFaultState.msg
  EStopState.msg
  BehaviorFaultState.msg
  FootState.msg
  RobotState.msg
  zedInfo.msg
  BraitenMsg.msg
  thermalInfo.msg
  targetInfo.msg
  
)

## Generate added messages and services with any dependencies listed here
generate_messages(
  DEPENDENCIES
  std_msgs
  geometry_msgs
  sensor_msgs
  diagnostic_msgs
)

include_directories(
  ${catkin_INCLUDE_DIRS}
)

catkin_package(
  CATKIN_DEPENDS message_runtime std_msgs geometry_msgs sensor_msgs
)
