#include <VHACD.h>
#include "common.h"
#include "logger.h"
#include "callback.h"

extern "C"
{
    EXPORT VHACD_Context *VHACD_CreateContext(bool async, void *userdata, UserCallbackFunction callback, UserLoggerFunction logger)
    {
        return new VHACD_Context(async, userdata, callback, logger);
    }

    EXPORT void VHACD_GetUserPointers(VHACD_Context *self, VHACD::IVHACD::IUserCallback **callback, VHACD::IVHACD::IUserLogger **logger)
    {
        *callback = self->callback;
        *logger   = self->logger;
    }

    EXPORT void VHACD_GetVersion(int *major, int *minor)
    {
        *major = VHACD_VERSION_MAJOR;
        *minor = VHACD_VERSION_MINOR;
    }

    EXPORT void VHACD_Cancel(VHACD_Context *self)
    {
        self->api->Cancel();
    }

    EXPORT bool VHACD_Compute(VHACD_Context *self, double const *const points, uint32_t const countPoints, uint32_t const *const triangles, uint32_t const countTriangles, VHACD::IVHACD::Parameters const *params)
    {
        return self->api->Compute(points, countPoints, triangles, countTriangles, *params);
    }

    EXPORT uint32_t VHACD_GetNConvexHulls(VHACD_Context *self)
    {
        return self->api->GetNConvexHulls();
    }

    EXPORT void VHACD_GetConvexHull(VHACD_Context *self, uint32_t const index, VHACD::IVHACD::ConvexHull *ch)
    {
        self->api->GetConvexHull(index, *ch);
    }

    EXPORT void VHACD_Release(VHACD_Context *self)
    {
        self->api->Release();
        delete self;
    }

    EXPORT bool VHACD_OCLInit(VHACD_Context *self, void *const oclDevice)
    {
        return self->api->OCLInit(oclDevice, self->logger);
    }

    EXPORT bool VHACD_OCLRelease(VHACD_Context *self)
    {
        return self->api->OCLRelease(self->logger);
    }

    EXPORT bool VHACD_ComputeCenterOfMass(VHACD_Context *self, double *centerOfMass)
    {
        return self->api->ComputeCenterOfMass(centerOfMass);
    }

    EXPORT bool VHACD_IsReady(VHACD_Context *self)
    {
        return self->api->IsReady();
    }
}
