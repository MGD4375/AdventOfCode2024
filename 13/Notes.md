# Day 13
Day 13 looks like simultaneous equations, so lets take it out of code to work through.
We know
`TargetX = (AXMove * APress) + (BXMove * BPress)`
`TargetY = (AYMove * APress) + (BYMove * BPress)`

# Isolate a variable
`(TargetX - (BXMove * BPress)) / AXMove = APress`

# Sub into the other equation
`TargetY = (AYMove * (TargetX - (BXMove * BPress)) / AXMove) + (BYMove * BPress)`

...

I've lost a bit of steam here.