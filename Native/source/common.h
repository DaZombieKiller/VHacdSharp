#pragma once

#include <VHACD.h>

#define EXPORT __declspec(dllexport)
#define IMPORT __declspec(dllimport)

typedef void(*UserCallbackFunction)(void *, double const, double const, double const, char const *const, char const *const);
typedef void(*UserLoggerFunction)(void *, char const *const);

struct VHACD_Context
{
    void                         *ud;
    VHACD::IVHACD                *api;
    VHACD::IVHACD::IUserCallback *callback;
    VHACD::IVHACD::IUserLogger   *logger;

    VHACD_Context(bool, void *, UserCallbackFunction, UserLoggerFunction);
    ~VHACD_Context();
};
