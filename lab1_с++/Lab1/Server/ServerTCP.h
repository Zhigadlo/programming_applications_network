#pragma once
#include <winsock2.h>
#include <thread>
#include <iostream>
#include <string>
#include <iomanip>
#include <sstream>

class Server
{
public:
	Server(int port, int clientCount);
	std::thread* listenClients(double start, double end, int iters, double alpha, double* minX, double* minY);
	double getMinIndex(double* arr);
	~Server();
	int clientFinished;
private:
	SOCKET sock;
	SOCKET* clients;
	std::thread* clientThreads;
	WSADATA wsaData;
	WORD sockVer;
	int retVal;
	int clientCount;
	double square;
};

void ProcessingClients(SOCKET client, int count, int index, int* clientFinished, double start, double end, int iters, double alpha, double* minX, double* minY);