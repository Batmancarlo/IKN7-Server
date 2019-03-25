using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace tcp
{
	class file_client
	{
		/// <summary>
		/// The PORT.
		/// </summary>
		const int PORT = 9000;
		/// <summary>
		/// The BUFSIZE.
		/// </summary>
		/// 
		/// 
		private string ServerIP;
		private string filePathFromServer;
		private string filePathToSave = "/home/ikn/";
		private string file;
		const int BUFSIZE = 1000;


		System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();

		void ConfiguratePathAndIp(string[] args)
		{
			if (args[0] != null)
				ServerIP = args[0];
			else
				Console.WriteLine(" << Write an ip in input argument");

			if (args[1] != null)
				filePathFromServer = args[1];
			else
				Console.WriteLine(" << Write an path and file in input argument");

			file = LIB.extractFileName(filePathFromServer);
			filePathToSave += file;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="file_client"/> class.
		/// </summary>
		/// <param name='args'>
		/// The command-line arguments. First ip-adress of the server. Second the filename
		/// </param>
		private file_client(string[] args)
		{
			
			//ConfiguratePathAndIp(new string[] { "10.0.0.1", "/home/ikn/TheFile.jpeg" });
            
			ConfiguratePathAndIp(args);
			clientSocket.Connect(ServerIP, PORT);

			NetworkStream serverStream = clientSocket.GetStream();
			//byte[] outStream = System.Text.Encoding.ASCII.GetBytes("TheFile" + "$");

			receiveFile(filePathFromServer, serverStream);


			// TO DO Your own code
		}

		/// <summary>
		/// Receives the file.
		/// </summary>
		/// <param name='fileName'>
		/// File name.
		/// </param>
		/// <param name='io'>
		/// Network stream for reading from the server
		/// </param>
		private void receiveFile(String fileName, NetworkStream io)
		{

			LIB.writeTextTCP(io, fileName);


			long filesize = LIB.getFileSizeTCP(io);
			Console.WriteLine(filesize.ToString());

			byte[] Recieved = new byte[filesize];
			int offset = 0;

			for (int i = (int)filesize; i != 0;)
			{
				if (i > BUFSIZE)
				{
					io.Read(Recieved, offset, (int)BUFSIZE);
					offset += BUFSIZE;
					i -= BUFSIZE;
				}
				else
				{
					io.Read(Recieved, offset, i);
					i -= i;


				}
				File.WriteAllBytes(filePathToSave, Recieved);

			}
			Console.Write("File saved");

		}


			/// <summary>
			/// The entry point of the program, where the program control starts and ends.
			/// </summary>
			/// <param name='args'>
			/// The command-line arguments.
			/// </param>
			public static void Main(string[] args)
			{
				Console.WriteLine("Client starts...");
				new file_client(args);
			}
		}
	}

