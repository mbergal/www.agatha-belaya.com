Properties {
    $x = 200
}

Task default -Depends Nested2CheckX

Task Nested2CheckX{
    Assert ($x -eq 200) '$x was not 200'
}
