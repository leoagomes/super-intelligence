// dllmain.cpp : Defines the entry point for the DLL application.
#include "stdafx.h"

#include <Windows.h>
#include <stdlib.h>
#include <easyhook.h>

#define DllExport __declspec(dllexport)
#define CExport extern "C"

bool updateRandomSeed = false;
unsigned int randomSeed = 1;

DWORD baseTick;
double speed = 1.0;

CExport DllExport void __cdecl SetSpeed(double targetSpeed)
{
	baseTick = GetTickCount();
	speed = targetSpeed;
}

CExport DllExport double __cdecl GetSpeed()
{
	return speed;
}

CExport DllExport void __cdecl SetRandomSeed(unsigned int seed)
{
	updateRandomSeed = true;
	randomSeed = seed;
}

CExport int __cdecl patchedRand()
{
	if (updateRandomSeed) 
	{
		srand(randomSeed);
		updateRandomSeed = false;
	}
	return rand();
}

CExport DWORD __stdcall patchedGetTickCount()
{
	MessageBoxA(0, "Salve", 0, 0);
	return baseTick + 0;
}

void HookRand()
{
	HOOK_TRACE_INFO hHook = {NULL};

	NTSTATUS result = LhInstallHook(
		GetProcAddress(GetModuleHandle(TEXT("msvcr100")), "rand"),
		patchedRand,
		NULL,
		&hHook);
	if (FAILED(result))
	{
		MessageBoxA(0, "Failed installing random hook!", 0, 0);
	}

	ULONG aclEntries[] = { 0 };
	LhSetExclusiveACL(aclEntries, 1, &hHook);
}

void HookGetTickCount()
{
	HOOK_TRACE_INFO hHook = {NULL};

	NTSTATUS result = LhInstallHook(
		GetProcAddress(GetModuleHandle(TEXT("kernel32")), "GetTickCount"),
		patchedGetTickCount,
		NULL,
		&hHook);
	if (FAILED(result))
	{
		MessageBoxA(0, "Failed installing GetTickCount hook!", 0, 0);
	}

	ULONG aclEntries[] = { 0 };
	LhSetExclusiveACL(aclEntries, 1, &hHook);
}

CExport DllExport void __cdecl HookFunctions()
{
	HookRand();
	//HookGetTickCount();
}

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
                     )
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
		baseTick = GetTickCount();
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}

