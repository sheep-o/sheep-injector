#include <Windows.h>

extern "C"
__declspec(dllexport)
bool
__cdecl
inject(int pid, const char* dllPath)
{
	int pathSize = strlen(dllPath) + 1;

	HANDLE hProc = OpenProcess(PROCESS_ALL_ACCESS, FALSE, pid);
	if (!hProc) return FALSE;

	LPVOID pDllPath = (char*)VirtualAllocEx(hProc, NULL, pathSize, MEM_COMMIT | MEM_RESERVE, PAGE_EXECUTE_READWRITE);
	WriteProcessMemory(hProc, pDllPath, dllPath, pathSize, NULL);

	HMODULE hKernel32 = GetModuleHandle(L"Kernel32");
	VOID *pLoadLibA = GetProcAddress(hKernel32, "LoadLibraryA");

	CreateRemoteThread(hProc, NULL, 0, (LPTHREAD_START_ROUTINE)pLoadLibA, pDllPath, 0 ,NULL);

	return TRUE;
}