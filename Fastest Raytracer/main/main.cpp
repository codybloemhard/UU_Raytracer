#define GLEW_STATIC
#include <iostream>
#include <glm/glm.hpp>
#include <GL/glut.h>
#include "../raytrace/raytrace_program.h"
#include "../raytrace/primitives.h"

using namespace glm;

raytrace::World world = raytrace::World(raytrace::Camera(glm::vec3(0, 1, 0), glm::quat(), 1, 1));
raytrace::RaytraceProgram program(DEMO_WIDTH, DEMO_HEIGHT, world);


void on_keyboard(unsigned char key, int x, int y) {
    switch(key) {
        case 27:
            exit(0);
        case 'w':
            program.world.objects[4]->set_position(program.world.objects[4]->get_position() + glm::vec3(0, 0, 0.1f));
            program.world.changed = true;
            break;
        case 's':
            program.world.objects[4]->set_position(program.world.objects[4]->get_position() + glm::vec3(0, 0, -0.1f));
            program.world.changed = true;
            break;
        case 'a':
            program.world.objects[4]->set_position(program.world.objects[4]->get_position() + glm::vec3(0.1f, 0, 0));
            program.world.changed = true;
            break;
        case 'd':
            program.world.objects[4]->set_position(program.world.objects[4]->get_position() + glm::vec3(-0.1f, 0, 0));
            program.world.changed = true;
            break;
        default:
            break;
    }
}

void on_special(int key, int x, int ) {
    switch(key) {
        case GLUT_KEY_UP:
            program.world.camera.rotate_by({DEMO_ROTATE_DELTA,0, 0});
            program.world.changed = true;
            break;
        case GLUT_KEY_DOWN:
            program.world.camera.rotate_by({-DEMO_ROTATE_DELTA,0, 0});
            program.world.changed = true;
            break;
        case GLUT_KEY_LEFT:
            program.world.camera.rotate_by({0,-DEMO_ROTATE_DELTA, 0});
            program.world.changed = true;
            break;
        case GLUT_KEY_RIGHT:
            program.world.camera.rotate_by({0,DEMO_ROTATE_DELTA, 0});
            program.world.changed = true;
            break;
        default:
            break;
    }
}

void on_display() {
    program.Clear();
    program.Draw();

    glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

    //glRasterPos2i(0, 0);
    glDrawPixels(DEMO_WIDTH, DEMO_HEIGHT, GL_RGB, GL_UNSIGNED_BYTE, program.raster.pixels);

    glFlush();
    glFinish();
    glutPostRedisplay();
    glutSwapBuffers();
}

void init_world() {
    auto plane = new raytrace::Plane(glm::vec3(0, 0 ,0), glm::quat());
    plane->mat.diffuse = {0.2, 0.2f, 0.2f};
    program.world.add_object(plane);

    auto sphere_matte = new raytrace::Sphere({-2.5f, 1, 5.5f}, 1);
    sphere_matte->mat.diffuse = {1, 0.2f, 0.2f};
    sphere_matte->mat.glossy = true;
    sphere_matte->mat.specular = sphere_matte->mat.diffuse * .5f;
    sphere_matte->mat.shinyness = 8;
    program.world.add_object(sphere_matte);

    auto sphere_mirror = new raytrace::Sphere({0, 1, 6}, 1);
    sphere_mirror->mat.glossy = true;
    sphere_mirror->mat.specular = sphere_mirror->mat.diffuse * .2f;
    sphere_mirror->mat.shinyness = 16;
    sphere_mirror->mat.is_mirror = true;
    program.world.add_object(sphere_mirror);

    auto sphere_glossy = new raytrace::Sphere({2.5f, 1, 5.5f}, 1);
    sphere_glossy->mat.diffuse = {0.2f, 0.2f, 1};
    sphere_glossy->mat.glossy = true;
    sphere_glossy->mat.specular = sphere_glossy->mat.diffuse * .2f;
    sphere_glossy->mat.shinyness = 16;
    sphere_glossy->mat.is_dielectic = true;
    sphere_glossy->mat.reflectivity = 0.6f;
    program.world.add_object(sphere_glossy);

    auto sphere_glass = new raytrace::Sphere({0, 1, 1.5}, 1);
    sphere_glass->mat.absorb = glm::vec3(4, 4, 1.5f) * 0.1f;
    sphere_glass->mat.glossy = true;
    sphere_glass->mat.specular = sphere_glossy->mat.diffuse * .2f;
    sphere_glass->mat.shinyness = 16;
    sphere_glass->mat.is_dielectic = true;
    sphere_glass->mat.is_refractive = true;
    sphere_glass->mat.refraction_index = 1.2f;
    program.world.add_object(sphere_glass);

    auto light = new raytrace::Light({0, 7, 0.5f}, {1,1,1}, 15);
    program.world.add_light(light);
}


int main(int argc, char **argv) {
    glutInit(&argc, argv);
    glutInitDisplayMode(GLUT_DOUBLE | GLUT_RGB);
    glutInitWindowSize(DEMO_WIDTH, DEMO_HEIGHT);
    glutInitWindowPosition(0, 0);
    glutCreateWindow(DEMO_TITLE);

    glutDisplayFunc(on_display);
    glutKeyboardFunc(on_keyboard);
    glutSpecialFunc(on_special);

    glDisable(GL_DEPTH_TEST);
    glClearColor(0.0f, 0.0f, 0.0f, 0.0f);
    glMatrixMode(GL_PROJECTION);
    glLoadIdentity();
    glViewport(0, 0, DEMO_WIDTH, DEMO_HEIGHT);
    gluOrtho2D(0, DEMO_WIDTH, 0, DEMO_HEIGHT);
    glPixelZoom(1, -1);
    glRasterPos2i(0, DEMO_HEIGHT - 1);

    init_world();
    program.Init();

    glutMainLoop();
}