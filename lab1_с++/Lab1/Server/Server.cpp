#include "ServerTCP.h"
#include <string>
#include <regex>

bool is_number(const std::string& s)
{
	std::regex r("^-?\\d+(\\.\\d+)?$");
	return std::regex_match(s, r);
}

int main()
{
	const int PORT = 5000;
	int clientCount = 0;
	std::string input;
	std::string a;
	std::string b;
	std::string iters;
	std::string alpha;
	double* minX;
	double* minY;

	do
	{
		std::cout << "Enter clients count:" << std::endl;
		std::cin >> input;
	} while (input[0] < '0' || input[0] > '9');

	clientCount = stoi(input);

	minX = new double[clientCount];
	minY = new double[clientCount];

	Server server(PORT, clientCount);

	do
	{
		std::cout << "Enter start point:" << std::endl;
		std::cin >> a;
	} while (!is_number(a));

	do
	{
		std::cout << "Enter end point:" << std::endl;
		std::cin >> b;
	} while (!is_number(b));

	do
	{
		std::cout << "Enter count of iterations:" << std::endl;
		std::cin >> iters;
	} while (!is_number(iters));

	do
	{
		std::cout << "Enter alpha:" << std::endl;
		std::cin >> alpha;
	} while (!is_number(alpha));

	std::thread* threads = server.listenClients(stod(a), stod(b), stoi(iters), stod(alpha), minX, minY);

	while (server.clientFinished != clientCount);

	int minIndx = server.getMinIndex(minY);

	std::cout << "minimal value of function: " << std::to_string(minX[minIndx]) + " " + std::to_string(minY[minIndx]) << std::endl;

	int tmp = getchar();

	return 0;
}
