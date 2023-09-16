#include "ServerTCP.h"

Server::Server(int port, int clientCount)
{
	square = 0.0;
	clientFinished = 0;
	this->clientCount = clientCount;
	clients = new SOCKET[clientCount];
	clientThreads = new std::thread[clientCount];
	sockVer = MAKEWORD(2, 2);

	retVal = WSAStartup(sockVer, &wsaData);
	//создание сокета (семейство протокола, потоковый сокет, протокол)
	sock = socket(PF_INET, SOCK_STREAM, IPPROTO_TCP);
	if (sock == INVALID_SOCKET)
	{
		//ошибка создания сокета
		std::cout << "Unable to create socket" << std::endl;
		WSACleanup();//Деинициаоизация бибилиотеки Winsoock
	}
	//описывает сокет для работы с протоколом 
	SOCKADDR_IN sin;
	sin.sin_family = PF_INET;
	sin.sin_port = htons(port);
	sin.sin_addr.s_addr = INADDR_ANY;
	//вызываем bind для связывания
	retVal = bind(sock, (LPSOCKADDR)&sin, sizeof(sin));
	if (retVal == SOCKET_ERROR)
	{
		std::cout << "Unable to bind" << std::endl;
		WSACleanup();//Деинициаоизация бибилиотеки Winsoock
	}
}

std::thread* Server::listenClients(double start, double end, int iters, double alpha, double* minX, double* minY)
{
	for (int i = 0; i < clientCount; i++)
	{
		int retVal = listen(sock, 10);
		if (retVal == SOCKET_ERROR)
		{
			std::cout << "Unable to listen" << std::endl;
		}

		clients[i] = accept(sock, NULL, NULL);

		if (clients[i] == INVALID_SOCKET)
		{
			std::cout << "Unable to accept" << std::endl;
		}

		clientThreads[i] = std::thread(ProcessingClients, clients[i], clientCount, i, &clientFinished, start, end, iters, alpha, minX, minY);
		clientThreads[i].detach();
	}

	return clientThreads;
}

double Server::getMinIndex(double* arr) {
	double min = arr[0];
	int minIndx = 0;

	for (int i = 0; i < sizeof(arr); i++) 
	{
		if (arr[i] < min)
		{
			min = arr[i];
			minIndx = i;
		}
	}

	return minIndx;
}

void ProcessingClients(SOCKET client, int count, int index, int* clientFinished, double start, double end, int iters, double alpha, double* minX, double* minY) {
	const int RESVBUF = 32;
	double step = (end - start) / count;
	double start_point = step * (double)index + start;

	std::stringstream stream_x1;
	stream_x1 << std::fixed << std::setprecision(3) << start_point;

	std::stringstream stream_x2;
	stream_x2 << std::fixed << std::setprecision(3) << iters;

	std::stringstream stream_x3;
	stream_x3 << std::fixed << std::setprecision(3) << alpha;

	std::string sendMessage = stream_x1.str() + " " + stream_x2.str() + " " + stream_x3.str();

	std::cout << "Connection with " << index << " client established successfully. Sent data: " << sendMessage << std::endl;

	int err = 0;
	//
	err = send(client, sendMessage.c_str(), sendMessage.length(), 0);

	char receiveMessageBuf[RESVBUF];
	//
	err = recv(client, receiveMessageBuf, RESVBUF, 0);

	std::string message = "";
	for (int i = 0; i < RESVBUF; i++)
	{
		message += receiveMessageBuf[i];
	}

	double x = atof(message.substr(0, message.find(' ')).c_str());
	double y = atof(message.substr(message.find(' ') + 1, message.length() - message.find(' ') - 1).c_str());

	std::cout << "Answer from " << index << " client. Recieved data: " << std::to_string(x) + " " + std::to_string(y) << std::endl;
	minX[index] = x;
	minY[index] = y;
	*clientFinished += 1;
}

Server::~Server()
{
	for (int i = 0; i < clientCount; i++)
	{
		closesocket(clients[i]);
	}

	delete[] clientThreads;
	delete[] clients;
	closesocket(sock);
	WSACleanup();
}
