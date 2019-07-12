#include <VHACD.h>
#include "logger.h"
#include "callback.h"
#include "common.h"

VHACD_Context::VHACD_Context(bool async, void* ud, UserCallbackFunction callback, UserLoggerFunction logger)
{
    this->ud       = ud;
    this->api      = async ? VHACD::CreateVHACD_ASYNC() : VHACD::CreateVHACD();
    this->callback = new DelegateUserCallback(callback, this);
    this->logger   = new DelegateUserLogger(logger, this);
}

VHACD_Context::~VHACD_Context()
{
    delete callback;
    delete logger;
}
