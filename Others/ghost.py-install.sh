yum install xorg-x11-server-Xvfb
yum install epel-release
yum update
yum qt-webkit-devel

pip install pyside
pip install ghost

python:
from ghost import Ghost
ghost = Ghost()
session = ghost.start()
page, exres = session.open('https://www.google.com')
session.evaluate('document.body.innerHTML')
session.close()
