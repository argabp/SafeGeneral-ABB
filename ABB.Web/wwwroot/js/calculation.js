
class Calculation {
    constructor() {
        this._symbols = {};
        this.defineOperator("!", this.factorial, "postfix", 6);
        this.defineOperator("^", Math.pow, "infix", 5, true);
        this.defineOperator("*", this.multiplication, "infix", 4);
        this.defineOperator("/", this.division, "infix", 4);
        this.defineOperator("+", this.last, "prefix", 3);
        this.defineOperator("-", this.negation, "prefix", 3);
        this.defineOperator("+", this.addition, "infix", 2);
        this.defineOperator("-", this.subtraction, "infix", 2);
        this.defineOperator(",", Array.of, "infix", 1);
        this.defineOperator("(", this.last, "prefix");
        this.defineOperator(")", null, "postfix");
        this.defineOperator("min", Math.min);
        this.defineOperator("sqrt", Math.sqrt);
        this.afterValue = false;
    }
    // Method allowing to extend an instance with more operators and functions:
    defineOperator(symbol, f, notation = "func", precedence = 0, rightToLeft = false) {
        // Store operators keyed by their symbol/name. Some symbols may represent
        // different usages: e.g. "-" can be unary or binary, so they are also
        // keyed by their notation (prefix, infix, postfix, func):
        if (notation === "func") precedence = 0;
        this._symbols[symbol] = Object.assign({}, this._symbols[symbol], {
            [notation]: {
                symbol, f, notation, precedence, rightToLeft,
                argCount: 1 + (notation === "infix")
            },
            symbol,
            regSymbol: symbol.replace(/[\\^$*+?.()|[\]{}]/g, '\\$&')
                + (/\w$/.test(symbol) ? "\\b" : "") // add a break if it's a name 
        });
    }
    last(...a) { return a[a.length - 1] }
    negation(a) { return -a }
    addition(a, b) { return a + b }
    subtraction(a, b) { return a - b }
    multiplication(a, b) { return a * b }
    division(a, b) { return a / b }
    factorial(a) {
        if (a % 1 || (+a < 0)) return NaN
        if (a > 170) return Infinity;
        let b = 1;
        while (a > 1) b *= a--;
        return b;
    }
    calculate(expression) {
        let match;
        const values = [],
            operators = [this._symbols["("].prefix],
            exec = _ => {
                let op = operators.pop();
                values.push(op.f(...[].concat(...values.splice(-op.argCount))));
                return op.precedence;
            },
            error = msg => {
                // dimark dulu ya let notation = match ? match.index : expression.length;
                // dimark dulu  return `${msg} at ${notation}:\n${expression}\n${' '.repeat(notation)}^`;
                return '0';
            },
            pattern = new RegExp(
                // Pattern for numbers
                "\\d+(?:\\.\\d+)?|"
                // ...and patterns for individual operators/function names
                + Object.values(this._symbols)
                    // longer symbols should be listed first
                    .sort((a, b) => b.symbol.length - a.symbol.length)
                    .map(val => val.regSymbol).join('|')
                + "|(\\S)", "g"
            );
        pattern.lastIndex = 0; // Reset regular expression object
        this.calculateDoWhile(expression, match, values, operators, exec, error, pattern);
        var result;
        if (operators.length)
            result = error("Missing closing parenthesis");
        else if (match) {
            result = error("Too many closing parentheses")
        }
        else {
            result = values.pop() // All done!
        }
        return result
    }
    calculateDoWhile(expression, match, values, operators, exec, error, pattern) {
        this.afterValue = false;
        do {
            match = pattern.exec(expression);
            const [token, bad] = match || [")", undefined],
                notNumber = this._symbols[token],
                notNewValue = notNumber && !notNumber.prefix && !notNumber.func,
                notAfterValue = !notNumber || !notNumber.postfix && !notNumber.infix;
            // Check for syntax errors:
            if (bad || (this.afterValue ? notAfterValue : notNewValue)) return error("Syntax error");
            if (this.afterValue) {
                this.afterValueFunc(notNumber, operators, exec);
            } else if (notNumber) { // prefix operator or function
                this.notNumberFunc(notNumber, operators, match, pattern, expression, error);
            } else { // number
                values.push(+token);
                this.afterValue = true;
            }
        } while (match && operators.length);
    }
    notNumberFunc(notNumber, operators, match, pattern, expression, error) {
        operators.push(notNumber.prefix || notNumber.func);
        if (notNumber.func) { // Require an opening parenthesis
            match = pattern.exec(expression);
            if (!match || match[0] !== "(") return error("Function needs parentheses")
        }
    }
    afterValueFunc(notNumber, operators, exec) {
        // We either have an infix or postfix operator (they should be mutually exclusive)
        const curr = notNumber.postfix || notNumber.infix;
        do {
            const prev = operators[operators.length - 1];
            if (((curr.precedence - prev.precedence) || prev.rightToLeft) > 0) break;
            // Apply previous operator, since it has precedence over current one
        } while (exec()); // Exit loop after executing an opening parenthesis or function
        this.afterValue = curr.notation === "postfix";
        if (curr.symbol !== ")") {
            operators.push(curr);
            // Postfix always has precedence over any operator that follows after it
            if (this.afterValue) exec();
        }
    }
}
var calculation = new Calculation(); // Create a singleton