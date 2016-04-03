# Angular 2 Hack

### angular2-polyfills.js

###### cannot do clearTimeout correctly

(beta.11 #142, beta.12 #147)

`task` lacking null validation; fix: add `if (task)` ahead the line `task.zone.cancelTask(task);`
