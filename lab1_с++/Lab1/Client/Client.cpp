#include <iostream>
#include <cmath>
#include <cstdlib>
#include "ClientTCP.h"

double function(double x) {
	return  sin(x)*pow(x, 2) - 10;
}

double df(double x) {
	return cos(x)*pow(x,2) - 2*x*sin(x);
}

double gradientDescent(double x_start, int iters, double alpha) {
	double x = x_start;

	for (int i = 0; i < iters; i++) {
		x = x - alpha * df(x);
	}

	return x;
}

int main()
{
	const int PORT = 5000;

	std::string message;
	int iters;
	double start_point, x, y, alpha;

	ClientTCP client(PORT);
	message = client.receiveMessage(15);

	start_point = atof(message.substr(0, message.find(' ')).c_str());
	iters = atoi(message.substr(message.find(' ') + 1, message.length() - message.find(' ') - 1).c_str());
	alpha = atoi(message.substr(message.find(' ') + 2, message.length() - message.find(' ') - 2).c_str());

	std::cout << "start_point:    " << start_point << std::endl;
	std::cout << "iters:    " << iters << std::endl;
	std::cout << "alpha: " << alpha << std::endl;

	x = gradientDescent(start_point, iters, alpha);
	y = function(x);

	message = std::to_string(x) + " " + std::to_string(y);

	std::cout << "Minimum of function: " << message << std::endl;

	client.sendMessage(message);

	int tmp = getchar();

	return 0;
}