#pragma once

#include <VHACD.h>
#include "common.h"

class DelegateUserCallback : public VHACD::IVHACD::IUserCallback
{
    VHACD_Context       *m_Context;
    UserCallbackFunction m_Update;

public:
    DelegateUserCallback(UserCallbackFunction update, VHACD_Context *context)
        : m_Update(update), m_Context(context)
    {
    }

    ~DelegateUserCallback()
    {
    }

    void Update(double const overallProgress, double const stageProgress, double const operationProgress, char const *const stage, char const *const operation)
    {
        m_Update(m_Context->ud, overallProgress, stageProgress, operationProgress, stage, operation);
    }
};
