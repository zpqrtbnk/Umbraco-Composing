
Register
--------

A register can register:
* A service type as its own implementation, with a lifecycle
* A service type with an implementation type, and a lifecycle
* A service type with an implementation factory, and a lifecycle

Factory
-------

A factory can:
* Get an implementation of a service type
* Get all implementations of a service type
* Try to get an implementation of a service type

When no implementation of a service has been registered, the container must throw.

When trying to get an implementation, it must return null instead.

When getting all implementations, it must return an empty enumerable instead.

Life Cycles
-----------

A container must support the following life cycles:
* Transient - create a new instance of the service each time the service is resoved
* Scope - create only one instance of the service per scope
* Singleton - create only one instance of the service per container

fixme / what about disposing of services? explicit release vs LightInject implicit?
fixle / what is LightInject's Request lifecycle?

Scope
-----

A container must support
* CreateScope() - returns a disposable object that releases the scope when disposed

Scopes can be nested

fixme / is it possible to create anything outside of a scope?
fixme / can a scope be web-request related?

Enumerables
-----------

A container must resolve IEnumerable<TService> and return all implementations of TService.

If no implementation of TService has been registered, the container must return an
empty IEnumerable<TService>.

Lazy
----

A container must resolve Lazy<TService> and return a Lazy producing TService.

Explicit registration
---------------------

Must register (IThing, Thing) for Thing to be an implementation of IThing.

Simply registering Thing is not enough (no implicit registration).