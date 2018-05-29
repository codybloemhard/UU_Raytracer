//
// Created by egordm on 16-5-2018.
//

#ifndef RAYTRACE_FILE_UTILS_H
#define RAYTRACE_FILE_UTILS_H

#include <fstream>
#include <iostream>

namespace raytrace { namespace utils { namespace file {
    std::string read_file(const char *filePath) {
        std::string content;
        std::ifstream fileStream(filePath, std::ios::in);

        if(!fileStream.is_open()) {
            std::cerr << "Could not read file " << filePath << ". File does not exist." << std::endl;
            return "";
        }

        std::string line;
        while(!fileStream.eof()) {
            std::getline(fileStream, line);
            content.append(line + "\n");
        }

        fileStream.close();
        return content;
    }
}}}

#endif //RAYTRACE_FILE_UTILS_H
