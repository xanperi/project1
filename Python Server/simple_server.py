#!/usr/bin/python
import http.server
import io
from os import curdir, sep, listdir, path

PORT_NUMBER = 33333

#This class will handle any incoming request from
#the browser 
class myHandler(http.server.SimpleHTTPRequestHandler):
	
	#Handler for the GET requests
	def do_GET(self):
		if self.path=="/":
			#print ("Path is => " + self.path)
			try:
				#Check the file extension required and
				#set the right mime type

				newest = max(listdir('.'), key = path.getctime)
			
				#print("Sending file " + newest)
			
				mimetype='text/plain; charset=utf-8'

				self.send_response(200)
				self.send_header("Content-type", mimetype)
				self.send_header("Content-Length", len(newest) )
				self.end_headers()
				self.wfile.write(newest.encode('utf-8'))
				
				return
			except IOError:
				self.send_error(404,'File Not Found: %s' % self.path)
		else:
			super(myHandler, self).do_GET()
			


try:
	#Create a web server and define the handler to manage the
	#incoming request
	server = http.server.HTTPServer(('', PORT_NUMBER), myHandler)
	print ('Started httpserver on port ' , PORT_NUMBER)
	
	#Wait forever for incoming htto requests
	server.serve_forever()

except KeyboardInterrupt:
	print ('^C received, shutting down the web server')
	server.socket.close()