import cv2

def crop(img):
    gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
    _, thresh = cv2.threshold(gray, 1, 255, cv2.THRESH_BINARY)
    _, cnts, hierarchy = cv2.findContours(thresh, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)
    min_x, min_y, max_x, max_y = gray.shape[0], gray.shape[1], 0, 0
    for c in cnts:
        x, y, w, h = cv2.boundingRect(c)
        if min_x > x:
            min_x = x
        if min_y > y:
            min_y = y
        if max_x < x + w:
            max_x = x + w
        if max_y < y + h:
            max_y = y + h
    
    return img[min_y: max_y, min_x: max_x]