using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace tcp
{
	class file_server
	{
		
		const int PORT = 9000;
	
		const int BUFSIZE = 1000;

		private file_server()
		{
			string ClientIP = "10.0.0.1";
			// TO DO Your own code
			UdpClient server = new UdpClient(PORT);
			IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Any, PORT);


			while (true)
			{
				byte[] CommandRecievedByte = server.Receive(ref serverEndPoint);

				string Command = Encoding.ASCII.GetString(CommandRecievedByte);

				if (Command == "l" || Command == "L")
				{
					SendLoadAverage(ClientIP);
				}
				else if (Command == "u" || Command == "U")
				{
					SendUpTime(ClientIP);
				}
				else
				{
					SendUnkownCommandError(ClientIP);
				}

			}
		}


			void SendLoadAverage(string IP)
			{
				string LoadAverage = File.ReadAllText("/proc/loadavg");
				byte[] LoadAverageByte = Encoding.ASCII.GetBytes(LoadAverage);
				UdpClient client = new UdpClient();
				IPEndPoint ClientEndPoint = new IPEndPoint(IPAddress.Parse(IP), PORT);
				client.Connect(ClientEndPoint);
				client.Send(LoadAverageByte, LoadAverageByte.Length);
				client.Close();
			    Console.WriteLine("Send {0}",LoadAverage);
			}


			void SendUpTime(string IP)
			{

				string UpTime = File.ReadAllText("/proc/uptime");
				byte[] UpTimeByte = Encoding.ASCII.GetBytes(UpTime);
				UdpClient client = new UdpClient();
				IPEndPoint ClientEndPoint = new IPEndPoint(IPAddress.Parse(IP), PORT);
				client.Connect(ClientEndPoint);
				client.Send(UpTimeByte, UpTimeByte.Length);
				client.Close();
			    Console.WriteLine("Send {0}",UpTime);
			}

			void SendUnkownCommandError(string IP)
			{
				string ErrorMessage = "Unknown Command";
				byte[] ErrorMessageByte = Encoding.ASCII.GetBytes(ErrorMessage);
				UdpClient client = new UdpClient();
				IPEndPoint ClientEndPoint = new IPEndPoint(IPAddress.Parse(IP), PORT);
				client.Connect(ClientEndPoint);
				client.Send(ErrorMessageByte, ErrorMessageByte.Length);
				client.Close();
				Console.WriteLine("Unkown Command Recieved, error message is send to client/n");
			}

		

		public static void Main(string[] args)
		{
			Console.WriteLine("Server starts...");
			new file_server();
		}
	}
}

