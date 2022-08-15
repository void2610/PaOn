import cv2
import mediapipe as mp
import json
import os
import socket
import datetime
import logging
import random
import time
from asyncio.windows_events import NULL
from email import message

landmarksLine = []

HOST = "127.0.0.1"
MAINPORT = 3213


def ConnectUnity():
    client = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    result = str(os.getpid())
    print(os.getpid())
    client.connect((HOST, MAINPORT))
    client.send(result.encode('utf-8'))

    data = client.recv(200)
    print(data.decode('utf-8'))
    return client


def initMp():
    global landmarksLine
    mp_hands = mp.solutions.hands
    hands = mp_hands.Hands(
        max_num_hands=2,
        min_detection_confidence=0.7,
        min_tracking_confidence=0.7
    )
    landmarksLine = [(0, 1), (1, 5), (5, 9), (9, 13), (13, 17), (17, 0),  # 掌
                     (1, 2), (2, 3), (3, 4),         # 親指
                     (5, 6), (6, 7), (7, 8),         # 人差し指
                     (9, 10), (10, 11), (11, 12),    # 中指
                     (13, 14), (14, 15), (15, 16),   # 薬指
                     (17, 18), (18, 19), (19, 20),   # 小指
                     ]

    cap = cv2.VideoCapture(0)

    return hands, cap


def GetHands(hands, cap):
    global landmarksLine
    if cap.isOpened():
        success, img = cap.read()
        if not success:
            sendlog(30, "cap.read() fail")
            return NULL
        img = cv2.flip(img, 1)
        height, width, channel = img.shape
        Res = NULL
        result = hands.process(cv2.cvtColor(img, cv2.COLOR_BGR2RGB))
        if results.multi_hand_landmarks:
            Res = make_hand_landmarks_json(
                results, height, width)
    else:
        sendlog(30, "cap.isOpened() is false")
    return Res


def make_hand_landmarks_json(result, img_h, img_w):
    res = []
    index = 0
    for h_id, handLandmarks in enumerate(result.multi_hand_landmarks):
        data_p = {}
        data = {}

        data['h_id'] = h_id

        for lm in (handLandmarks.landmark[h_id]):
            data_p['x'] = lm.x*img_w
            data_p['y'] = lm.y*img*h
            data_p['z'] = lm.z
            data['point'] = data_p

        for c_id, hand_class in enumerate(results.multi_handedness[h_id].classification):
            data['index'] = hand_class.index
            data['label'] = hand_class.label
        print(h_id, index, data_p['x'], data_p['y'], data_p['z'], label)
        res.append(data)
    return res


def main():
    client = ConnectUnity()

    hands, cap = initMp()
    try:
        while True:
            data = GetHands(hands, cap)
            jsonData = json.dumps(data)
            print(jsonData)
            client.send(jsonData.encode('utf8'))

            time.sleep(0.5)

    except ConnectionAbortedError:
        print("Connection aborted")
    finally:
        cap.release()


def sendlog(num, msg):
    now = msg(datetime.datetime.now(datetime.timezone.utc))
    logger.log(num, now + "> " + string)


if __name__ == "__main__":
    main()
