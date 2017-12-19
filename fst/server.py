from http.server import BaseHTTPRequestHandler, HTTPServer

import evaluate
import os
import base64
import json

# HTTPRequestHandler class
class testHTTPServer_RequestHandler(BaseHTTPRequestHandler):

  # GET
  def do_GET(self):

        impath = self.path[1:]

        try:
            q = json.loads(base64.b64decode(impath).decode("utf-8"))
        except:
            self.send_response(404)
            self.wfile.write(bytes("Not found", "utf-8"))
            print("Failed request")
            return

        evaluate.ffwd([q["path"]], ["out_dir/result.png"], "models/" + q["style"] + ".ckpt")

        self.send_response(200)

        # Send headers
        self.send_header('Content-type','image/png')
        self.end_headers()

        # Send message back to client
        message = "Hello world!"
        # Write content as utf-8 data
        with open("out_dir/result.png", "rb") as f:
            self.wfile.write(f.read())
        return

def run():
  print('starting server...')

  # Server settings
  # Choose port 8080, for port 80, which is normally used for a http server, you need root access
  server_address = ('127.0.0.1', 8081)
  httpd = HTTPServer(server_address, testHTTPServer_RequestHandler)
  print('running server...')
  httpd.serve_forever()


run()
