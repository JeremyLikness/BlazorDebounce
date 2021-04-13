# Blazor Debounce

Example of using a simpler way to handle "debouncing" input in a Blazor WebAssembly app.

`CompletionTemplate` provides a basic template for the input.

`AutoComplete` simply makes API calls on every keypress.

`TimerComplete` uses a timer to debounce by not fetching until 300ms of inactivity is detected.

`DebounceComplete` uses a simple queue flag to fetch until no longer required.