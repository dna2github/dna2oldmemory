import numpy
import cv2
import sys


pica_filename = sys.argv[0]
picb_filename = sys.argv[1]
dist = int(sys.argv[2])
a = cv2.imread(pica_filename)
b = cv2.imread(picb_filename)
ag = cv2.cvtColor(a, cv2.COLOR_BGR2GRAY)
bg = cv2.cvtColor(b, cv2.COLOR_BGR2GRAY)

sift = cv2.xfeatures2d.SIFT_create()
kpa,dea = sift.detectAndCompute(ag, None)
kpb,deb = sift.detectAndCompute(bg, None)

bf = cv2.BFMatcher()
matches = bf.knnMatch(dea, deb, k=2)

good = []
for group in matches:
  for one in group:
    if one.distance < dist:
      good.append(one)

ha, wa = ag.shape
hb, wb = bg.shape
out=numpy.zeros(shape=(ha, wa+wb))

out = cv2.drawMatches(ag,kpa,bg,kpb,good,outImg=out)
cv2.imwrite('/tmp/matches.jpg', out)
