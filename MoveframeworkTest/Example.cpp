// MoveFrameworkSDK.cpp : Defines the entry point for the console application.
//

#include <stdio.h>
#include <tchar.h>
#include "windows.h"

#include "IMoveManager.h"
#include "MoveFactory.h"
#include "MoveData.h"
#include "NavData.h"
#include "IMoveObserver.h"


class MoveObserver : public Move::IMoveObserver
{
	Move::IMoveManager* move;
	int numMoves;

public:
	MoveObserver()
	{
		move = Move::createDevice();

		numMoves = move->initMoves();
		move->initCamera(numMoves);

		move->subsribe(this);
	}

	void moveKeyPressed(int moveId, Move::MoveButton keyCode)
	{
		printf("MOVE id:%d   button pressed: %d\n", moveId, (int)keyCode);
		//exit if the Move button is pressed
		if (keyCode == Move::B_MOVE)
		{
			exit(0);
		}
	}

	void moveKeyReleased(int moveId, Move::MoveButton keyCode)
	{
		printf("MOVE id:%d   button released: %d\n", moveId, (int)keyCode);
	}

	void moveUpdated(int moveId, Move::MoveData data)
	{
		printf("MOVE id:%d   pos:%.2f %.2f %.2f   ori:%.2f %.2f %.2f %.2f   trigger:%d\n",
			moveId,
			data.position.x, data.position.y, data.position.z,
			data.orientation.w, data.orientation.v.x, data.orientation.v.y, data.orientation.v.z,
			data.trigger);
		//set rumble if cross is pressed
		if (data.isButtonPressed(Move::B_CROSS))
		{
			move->getMove(moveId)->setRumble(data.trigger);
		}
	}

	void navKeyPressed(int navId, Move::MoveButton keyCode)
	{
		printf("NAV id:%d   button pressed: %d\n", navId, (int)keyCode);
	}

	void navKeyReleased(int navId, Move::MoveButton keyCode)
	{
		printf("NAV id:%d   button released: %d\n", navId, (int)keyCode);
	}

	void navUpdated(int navId, Move::NavData data)
	{
		printf("NAV id:%d   trigger1: %d,  trigger2: %d, stick: %d,%d\n", navId, data.trigger1, data.trigger2, data.stickX, data.stickY);
	}
};

int main(int argc, char* argv[])
{
	MoveObserver* observer = new MoveObserver();

	getchar();
	return 0;
}

