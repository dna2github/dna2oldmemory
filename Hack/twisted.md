# Twisted Hack

### 16.0.0

###### static route example does not work

https://twistedmatrix.com/documents/current/web/howto/web-in-60/static-dispatch.html

```python
root = Resource()
root.putChild("foo", File("/tmp"))
```

However, in twisted source code:

```python
...
    def getChild(self, path, request):
        return NoResource("No such child resource.")
...
```

fix:

```python
class Root(Resource):
    def getChild(self, path, request):
        if path in self.children:
            return self.children[path]
        else:
            return NoResource("Not found.")


root = Root()
root.putChild("foo", File("/tmp"))
```

###### path name in different types for getChild - putChild of Resource for Python 3.x

```python
class Root(Resource):
    def getChild(self, path, request):
        print(type(path))
        return super().getChild(path, request)

    def putChild(self, path, child):
        print(type(path))
        return super().putChild(path, child)

root = Root()
```

Then `root.putChild("path", Resource())` it will print `str`. However, run server with reactor (e.g. `reactor.listenTCP(8080, Site(root)); ractor.run()`) and type url in some browser as `127.0.0.1:8080/path`, it will print `byte`.

Thus if do `path in self.children` in `getChild` will always get `False`. Try `path.decode('utf-8') in self.children`.

