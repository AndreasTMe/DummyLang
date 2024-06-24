# DummyLang

First attempt at creating a compiler's frontend.

### Table of Contents

- [Features](#features)
  - [Operators](#operators)
  - [Literals](#literals)
    - [Boolean Values](#boolean-values)
    - [Number Values](#number-values)
    - [Character Values](#character-values)
    - [String Values](#string-values)
  - [Variables](#variables)
  - [Functions](#functions)
  - [If-Else](#if-else)
  - [When-Is](#when-is)
  - [While Loop](#while-loop)

# Features

## Operators

| Operators          | Symbols                                                                   |
|--------------------|---------------------------------------------------------------------------|
| Primary            | x++, x--, x(y), x[i], x->y, x.y                                           |
| Unary              | +x, -x, ++x, --x, !x, ~x, *x, &x                                          |
| Range              | x..y                                                                      |
| Additive           | x+y, x-y                                                                  |
| Multiplicative     | x*y, x/y, x%y                                                             |
| Bitshift           | x>>y, x<<y                                                                |
| Relational         | x>y, x<y, x>=y, x<=y                                                      |
| Equality           | x==y, x!=y                                                                |
| Bitwise AND,XOR,OR | x&y, x^y, x\|y                                                            |
| Conditional AND,OR | x&&y, x\|\|y                                                              |
| Null-coalescing    | x??y                                                                      |
| Assignment         | x=y, x+=y, x-=y, x*=y, x/=y, x%=y, x>>=y, x<<=y, x&=y, x^=y, x\|=y, x??=y |

## Literals

### Boolean Values

```c#
true
false
```

### Number Values

```c#
0b0101      // Binary
0x12ac      // Hexadecimal

123         // Integer
123u        // Unsigned Integer
123l        // Long
123ul       // Unsigned Long

123.0f      // Float
123.0d      // Double
123.0m      // Decimal
123.0e-10f  // With Exponent
```

### Character Values

```c#
'a'
'\n'
'\x01ac'
```

### String Values

```c#
"some string"
"some string with escaped characters \n \x04bd 123"
```

## Variables

```c#
// Mutable variable declaration statement
var number: int32 = 1;

// Constant variable declaration statement
const pi: float32 = 3.14159f;

// TODO: Add pointers (thinking about syntax)
```

## Functions

```c#
// Function call expression
const result: int32 = Add(1, 2);

// Function declaration statement (form #1)
fun Add(lhs: int32, rhs: int32): int32
{
    // Return statement
    return lhs + rhs;
}

// Function declaration statement (form #2)
fun Subtract(lhs: int32, rhs: int32): int32 => lhs - rhs;
```

## If-Else

```c#
// Conditional if-else statement
if (value % 3 == 0)
{
    // Do something...
}
else if (value % 3 == 1)
{
    // Do something else...
}
else
{
    // Do something else...
}
```

## When-Is

```c#
const someNumber: int32 = GetNumber();

// When statement
when (someNumber)
{
    // Reserved inside the block:
    // is, not, and, or, value
    is 1 => DoStuff() // No block
    is >=2 and <5 =>
    {
        // With block
        DoOtherStuff();
        DoMoreStuff();
    }
    is 8 or (value % 2 == 1) => DoEvenMoreStuff()
    is not >50 => DoEvenEvenMoreStuff()
    _ => break;
}

// When expression
const result: int32 = when (something)
{
    // Every "is" must return a value
};
```

## While Loop

```c#
// While statement
while(times >= 0)
{
    if (somethingIsTrue)
    {
        // Break statement
        break;
    }
    
    if (somethingElseIsTrue)
    {
        times -= 2;
        // Continue statement
        continue;
    }
    
    times--;
}
```