#!/usr/bin/env python3
"""
Node for hosting WebController application

"""

import rospy
import cv2
from cv_bridge import CvBridge
from sensor_msgs.msg import Image
import threading
from flask import Flask, render_template, Response, request
import time

# Start ROS node in seperate thread
threading.Thread(target=lambda: rospy.init_node('web_controller', disable_signals=True)).start()

# Create a Flask Controller App
app = Flask(__name__)

class RosImage(object):
    def __init__(self):
        self.bridge = CvBridge()
        self.frame = None

    def callback(self, msg):
        # Convert ROS image to cv2 image and rotate 90 degrees cw, then convert to bytes
        cv_image = cv2.rotate(self.bridge.imgmsg_to_cv2(msg), cv2.ROTATE_90_CLOCKWISE)
        self.frame = cv2.imencode('.jpg', cv_image)[1].tobytes()

    def gen(self):
        while True:
            if self.frame is not None:
                yield (b'--frame\r\n'b'Content-Type: image/jpeg\r\n\r\n' + self.frame + b'\r\n')
            time.sleep(0.05)
            
# Create RosImage class which handles the ROS image callabacks
rosImage = RosImage()
sub_markers = rospy.Subscriber("spot_image", Image, rosImage.callback)


# Route creations
@app.route('/')
def index():
    """Home page."""
    return render_template('index.html')

@app.route('/video_feed', methods=['GET','POST'])
def video_feed():
    """Video streaming route."""
    return Response(rosImage.gen(), mimetype='multipart/x-mixed-replace; boundary=frame')



if __name__ == '__main__':
    # Run Flask app
    app.run(host='0.0.0.0', port=8000, debug=True, use_reloader=False)
