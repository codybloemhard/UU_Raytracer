// Made by Egor Dmitriev 6100120

w = 512,U = 1;

// Helpers
var math = Math;
var sqrt = math.sqrt;
var max = math.max;
var eplison = 1 / w;

// Pixel setting
var canvas = document.getElementById("A");
var context2d = canvas.getContext("2d");
var image_data = context2d.getImageData(0, 0, w, w);
var raw_data = image_data.data;

canvas.width = canvas.height = w;

var spheres = [
    1, [-3, 1, 8], 9, 0, 0,0,
    1, [0, 1, 8], 9, 9, 9,1,
    1, [3, 1, 8], 0, 0, 9,.3
];
var light_powar = 6e3;

function A_minus_Bk(A, B, k) {
    return [A[0] - B[0] * k, A[1] - B[1] * k, A[2] - B[2] * k];
}

function dot(A, B) {
    return A[0] * B[0] + A[1] * B[1] + A[2] * B[2];
}

function norm(A) {
    return A_minus_Bk([0, 0,0], A, -1/(B=sqrt(dot(A,A))));
}


var define_intersection = () => {
    if (f > eplison && f < T) {
        I = i||1;
        T = f;
        R = spheres[i+4]||0;
    }
};

var nearest_intersect = (O, D, t_max) => {
    T = t_max;

    // Intersect spheres
    for (I = i = 0; r = spheres[i++]; i += 5) {
        f = (f = dot(c = A_minus_Bk(spheres[i], O, 1), D)) - sqrt(r * r - dot(q = A_minus_Bk(c, D, f), q));
        define_intersection();
        if (I == i) {
            N = A_minus_Bk(X = A_minus_Bk(O, D, -T), spheres[i], 1);
            C = spheres[I + U];
        }
    }

    // Plane intersect. Normal is hardcoded = [0, 1, 0]
    f = -dot(O, r=[0, 1, 0]) / dot(D, r);
    define_intersection();
    if (I == i) {
        X = A_minus_Bk(O, D, -T);
        N = r;
        // Calculate checkerboard
        C = (~~X[0] + ~~X[2]) & 1 ? 0 : 9;
    }

    return I;
};

var trace_ray = (O, D, t_max) => {
    if(!nearest_intersect(O, D, t_max)) return 0;

    // Check light. Hardcoded position [0,7,0]
    k = max(0, dot(N, L = A_minus_Bk([0, 7, 0], X, 1)));

    // 13. 13 = A poor mans pi * 4
    return C * (1 - R) * !nearest_intersect(X, norm(L), B) * k * light_powar / (13 * dot(L, L)) // Diffuse
        + R * trace_ray(A_minus_Bk(X, g = A_minus_Bk(D, N, 2 * dot(D, N)), -eplison), g, t_max); // Reflections
};

var out_idx = 0;
for (y = h = w / 2; y-- > -h;) {
    for (x = -h; x++ < h;) {
        for (U = 0; ++U < 4;) {
            raw_data[out_idx++] = trace_ray([0, 1, 0], norm([x/w, y/w, 1]), w);
        }
        raw_data[out_idx++] = 255;
    }
}

context2d.putImageData(image_data, 0, 0);