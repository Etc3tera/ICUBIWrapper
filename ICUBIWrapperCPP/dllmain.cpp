// dllmain.cpp : Defines the entry point for the DLL application.
#include "stdafx.h"
#include "unicode/uchar.h"
#include "unicode/unistr.h"
#include "unicode/brkiter.h"
#include <string>

int listWordBoundaries(const UnicodeString& s, int *result) {
	UErrorCode status = U_ZERO_ERROR;
	BreakIterator* bi = BreakIterator::createWordInstance(Locale::getUS(), status);

	bi->setText(s);
	int32_t p = bi->first();
	int i = 0;

	while (p != BreakIterator::DONE) {
		result[i++] = p;
		p = bi->next();
	}
	delete bi;

	return i;
}

extern "C" {
	__declspec(dllexport) int GetWordBoundary(const char *input, int *result) {
		UnicodeString us = UnicodeString::fromUTF8(input);
		return listWordBoundaries(us, result);
	}

	__declspec(dllexport) int Sum(int a, int b) {
		return a + b;
	}
	
	__declspec(dllexport) void Foo(int *test) {
		for (int i = 0; i < 10; i++) {
			test[i] = i + 1;
		}
	}
}

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
					 )
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
	case DLL_PROCESS_DETACH:
		break;
	}
	return TRUE;
}

