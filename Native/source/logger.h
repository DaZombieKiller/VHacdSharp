#pragma once

#include <VHACD.h>
#include "common.h"

class DelegateUserLogger : public VHACD::IVHACD::IUserLogger
{
    VHACD_Context     *m_Context;
    UserLoggerFunction m_Log;

public:
    DelegateUserLogger(UserLoggerFunction log, VHACD_Context *context)
        : m_Log(log), m_Context(context)
    {
    }

    ~DelegateUserLogger()
    {
    }

    void Log(char const *const msg)
    {
        m_Log(m_Context->ud, msg);
    }
};
