# NSS Grammar Reference (from PyKotor PLY)

This document extracts the grammar rules from PyKotor's PLY-based parser to serve as a reference for the hand-written C# parser.

## Precedence

```python
precedence = (
    ("right", "=", "ADDITION_ASSIGNMENT_OPERATOR", "SUBTRACTION_ASSIGNMENT_OPERATOR", ...),
    ("right", "?"),
    ("left", "OR"),
    ("left", "AND"),
    ("left", "BITWISE_OR"),
    ("left", "BITWISE_XOR"),
    ("left", "BITWISE_AND"),
    ("left", "EQUALS", "NOT_EQUALS"),
    ("left", "GREATER_THAN", "LESS_THAN", "GREATER_THAN_OR_EQUALS", "LESS_THAN_OR_EQUALS"),
    ("left", "BITWISE_LEFT", "BITWISE_RIGHT", "BITWISE_UNSIGNED_RIGHT"),
    ("left", "ADD", "MINUS"),
    ("left", "MULTIPLY", "DIVIDE", "MOD"),
    ("right", "BITWISE_NOT", "NOT"),
    ("left", "INCREMENT", "DECREMENT"),
)
```

## Grammar Rules

### Top Level
```
code_root : code_root code_root_object
          | (empty)

code_root_object : function_definition
                 | include_script
                 | function_forward_declaration
                 | global_variable_declaration
                 | global_variable_initialization
                 | struct_definition
```

### Structs
```
struct_definition : STRUCT IDENTIFIER '{' struct_members '}' ';'

struct_members : struct_members struct_member
               | (empty)

struct_member : data_type IDENTIFIER ';'
```

### Includes
```
include_script : INCLUDE STRING_VALUE
```

### Global Variables
```
global_variable_initialization : data_type IDENTIFIER '=' expression ';'
                               | CONST data_type IDENTIFIER '=' expression ';'

global_variable_declaration : data_type IDENTIFIER ';'
                            | CONST data_type IDENTIFIER ';'
```

### Functions
```
function_forward_declaration : data_type IDENTIFIER '(' function_definition_params ')' ';'

function_definition : data_type IDENTIFIER '(' function_definition_params ')' '{' code_block '}'

function_definition_params : function_definition_params ',' function_definition_param
                           | function_definition_param
                           | (empty)

function_definition_param : data_type IDENTIFIER
                          | data_type IDENTIFIER '=' expression
```

### Code Blocks
```
code_block : code_block statement
           | (empty)

statement : expression_statement
          | declaration_statement
          | return_statement
          | break_statement
          | continue_statement
          | condition_statement
          | while_loop
          | do_while_loop
          | for_loop
          | switch_statement
          | scoped_block
          | nop_statement
```

### Control Flow
```
while_loop : WHILE_CONTROL '(' expression ')' '{' code_block '}'

do_while_loop : DO_CONTROL '{' code_block '}' WHILE_CONTROL '(' expression ')' ';'

for_loop : FOR_CONTROL '(' expression ';' expression ';' expression ')' '{' code_block '}'
         | FOR_CONTROL '(' declaration_statement expression ';' expression ')' '{' code_block '}'

scoped_block : '{' code_block '}'
```

### Statements
```
expression_statement : expression ';'

declaration_statement : data_type variable_declarators ';'
                      | CONST data_type variable_declarators ';'

variable_declarators : variable_declarators ',' variable_declarator
                     | variable_declarator

variable_declarator : IDENTIFIER
                    | IDENTIFIER '=' expression

return_statement : RETURN expression ';'
                 | RETURN ';'

break_statement : BREAK_CONTROL ';'

continue_statement : CONTINUE_CONTROL ';'

nop_statement : ';'
```

### Conditionals
```
condition_statement : if_statement

if_statement : IF_CONTROL '(' expression ')' '{' code_block '}' else_statement
             | IF_CONTROL '(' expression ')' statement else_statement

else_statement : ELSE_CONTROL '{' code_block '}'
               | ELSE_CONTROL statement
               | ELSE_CONTROL if_statement
               | (empty)

else_if_statement : ELSE_CONTROL IF_CONTROL '(' expression ')' '{' code_block '}'
                  | ELSE_CONTROL IF_CONTROL '(' expression ')' statement
```

### Switch
```
switch_statement : SWITCH_CONTROL '(' expression ')' '{' switch_blocks '}'

switch_blocks : switch_blocks switch_block
              | (empty)

switch_block : switch_labels block_statements

switch_labels : switch_labels switch_label
              | (empty)

switch_label : CASE expression ':'
             | DEFAULT ':'

block_statements : block_statements statement
                 | (empty)
```

### Expressions
```
expression : assignment_expression

assignment_expression : ternary_expression
                      | field_access '=' assignment_expression
                      | field_access ADDITION_ASSIGNMENT_OPERATOR assignment_expression
                      | field_access SUBTRACTION_ASSIGNMENT_OPERATOR assignment_expression
                      | field_access MULTIPLICATION_ASSIGNMENT_OPERATOR assignment_expression
                      | field_access DIVISION_ASSIGNMENT_OPERATOR assignment_expression
                      | field_access MOD_ASSIGNMENT_OPERATOR assignment_expression
                      | field_access BITWISE_AND_ASSIGNMENT_OPERATOR assignment_expression
                      | field_access BITWISE_OR_ASSIGNMENT_OPERATOR assignment_expression
                      | field_access BITWISE_XOR_ASSIGNMENT_OPERATOR assignment_expression
                      | field_access BITWISE_LEFT_ASSIGNMENT_OPERATOR assignment_expression
                      | field_access BITWISE_RIGHT_ASSIGNMENT_OPERATOR assignment_expression
                      | field_access BITWISE_UNSIGNED_RIGHT_ASSIGNMENT_OPERATOR assignment_expression

ternary_expression : logical_or_expression
                   | logical_or_expression '?' expression ':' ternary_expression

logical_or_expression : logical_and_expression
                      | logical_or_expression OR logical_and_expression

logical_and_expression : bitwise_or_expression
                       | logical_and_expression AND bitwise_or_expression

bitwise_or_expression : bitwise_xor_expression
                      | bitwise_or_expression BITWISE_OR bitwise_xor_expression

bitwise_xor_expression : bitwise_and_expression
                        | bitwise_xor_expression BITWISE_XOR bitwise_and_expression

bitwise_and_expression : equality_expression
                        | bitwise_and_expression BITWISE_AND equality_expression

equality_expression : relational_expression
                    | equality_expression EQUALS relational_expression
                    | equality_expression NOT_EQUALS relational_expression

relational_expression : shift_expression
                      | relational_expression GREATER_THAN shift_expression
                      | relational_expression LESS_THAN shift_expression
                      | relational_expression GREATER_THAN_OR_EQUALS shift_expression
                      | relational_expression LESS_THAN_OR_EQUALS shift_expression

shift_expression : additive_expression
                 | shift_expression BITWISE_LEFT additive_expression
                 | shift_expression BITWISE_RIGHT additive_expression
                 | shift_expression BITWISE_UNSIGNED_RIGHT additive_expression

additive_expression : multiplicative_expression
                   | additive_expression ADD multiplicative_expression
                   | additive_expression MINUS multiplicative_expression

multiplicative_expression : unary_expression
                          | multiplicative_expression MULTIPLY unary_expression
                          | multiplicative_expression DIVIDE unary_expression
                          | multiplicative_expression MOD unary_expression

unary_expression : postfix_expression
                 | MINUS unary_expression
                 | BITWISE_NOT unary_expression
                 | NOT unary_expression
                 | INCREMENT field_access
                 | DECREMENT field_access

postfix_expression : primary_expression
                   | field_access INCREMENT
                   | field_access DECREMENT
                   | postfix_expression '.' IDENTIFIER

primary_expression : IDENTIFIER
                   | function_call
                   | constant
                   | '(' expression ')'
                   | '[' FLOAT_VALUE ',' FLOAT_VALUE ',' FLOAT_VALUE ']'
                   | VECTOR '(' expression ',' expression ',' expression ')'
```

### Data Types
```
data_type : INT_TYPE
          | FLOAT_TYPE
          | OBJECT_TYPE
          | VOID_TYPE
          | EVENT_TYPE
          | EFFECT_TYPE
          | ITEMPROPERTY_TYPE
          | LOCATION_TYPE
          | STRING_TYPE
          | TALENT_TYPE
          | VECTOR_TYPE
          | ACTION_TYPE
          | STRUCT IDENTIFIER
          | IDENTIFIER
```

### Constants
```
constant : INT_VALUE
         | FLOAT_VALUE
         | STRING_VALUE
         | OBJECT_SELF
         | OBJECT_INVALID
```

### Function Calls
```
function_call : IDENTIFIER '(' function_call_params ')'

function_call_params : function_call_params ',' expression
                     | expression
                     | (empty)
```

### Field Access
```
field_access : IDENTIFIER
             | IDENTIFIER '.' IDENTIFIER
             | field_access '.' IDENTIFIER
```

## Notes

- This grammar is extracted from PyKotor's PLY parser
- The C# parser should match this grammar exactly
- Precedence rules are critical for correct expression parsing
- Empty productions are used for optional lists

