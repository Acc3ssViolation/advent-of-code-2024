##Ax = 94
##Ay = 34
##Bx = 22
##By = 67
##Px = 8400
##Py = 5400
Ax = 26
Ay = 66
Bx = 67
By = 21
Px = 12748
Py = 12176

def calc_m():
    num = Py - ((Ay * Px) / Ax)
    denum = By - ((Ay * Bx) / Ax)
    return num / denum

def calc_n(M):
    return (Px / Ax) - (Bx / Ax) * M

m = calc_m()
n = calc_n(m)

print(f"N={n} M={m}")
