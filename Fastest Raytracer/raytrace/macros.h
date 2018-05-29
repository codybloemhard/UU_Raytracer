//
// Created by egordm on 19-5-2018.
//

#ifndef RAYTRACE_MACROS_H
#define RAYTRACE_MACROS_H

#define CLAMP(x, low, high) ({\
  __typeof__(x) __x = (x); \
  __typeof__(low) __low = (low);\
  __typeof__(high) __high = (high);\
  __x > __high ? __high : (__x < __low ? __low : __x);\
  })

#define CHECK_BIT(var,pos) ((var) & (1<<(pos)))

#endif //RAYTRACE_MACROS_H
