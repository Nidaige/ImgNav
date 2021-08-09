from distutils.core import setup
from catkin_pkg.python_setup import generate_distutils_setup

d = generate_distutils_setup(
    packages=['spot_mil'],
    scripts=['scripts/spot_wrapper.py'],
    package_dir={'': 'src'}
)

setup(**d)
