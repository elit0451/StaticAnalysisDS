# Static Analysis Data Structure :microscope:
This repository experiments with **static analysis** by examining logic written in a **V**ery **S**imple **S**tructured
**L**anguage (**VSSL**).

</br>

----
## VSSL :notebook_with_decorative_cover:

> Key features

- The only data types are **booleans** (BOOLEAN) and **integers** (INTEGER).
- It supports blocks of code, curly braces around statements.
- It supports selections with the **IF** (...) code block and **IF** (...) code block **ELSE** code block statements
- It supports iterations with the **WHILE** (...) code block statement.
- Code blocks, selections, and iterations can be nested.

<details><summary>Example</summary> 

```java  
DEF X: Integer 
LET X = 10 
WHILE (X < 10) {
	LET X = X + 1
}
IF (X < 12) { 
	DEF Y: Integer
	Let Y = 5
	DEF Z: Boolean
	LET Z = True
}
ELSE {
	LET X = 90
}
LET Y = 10
LET Y = Y + 10

```
</details>

You can change the content of a `VSSL.txt` file and then change its [location](https://github.com/elit0451/StaticAnalysisDS/blob/0458b6c4ae1687ad42c27f2e2212b1199bd4744b/StaticAnalysisDS/Program.cs#L17)

</br>

---
## Running :running:
A recommendation for running the application is to have the **VSSL file** open on the side of the **started console application** and by stepping through the lines (pressing **Enter**) you would be able to check the *variables defined* and their *state*.


The console application will present you with something like:
> current line from VSSL.txt  
`LET Y = Y + 10`

> state of the variables
```
NAME    VALUE
X       10
Y       20
Z       True
```

</br>

___
> #### Assignment made by:   
`David Alves 👨🏻‍💻 ` :octocat: [Github](https://github.com/davi7725) <br />
`Elitsa Marinovska 👩🏻‍💻 ` :octocat: [Github](https://github.com/elit0451) <br />
> Attending "Discrete Mathematics" course of Software Development bachelor's degree
