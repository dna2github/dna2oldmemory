"""
# modify project/settings.py
DATABASES = {
    'default': {
        'ENGINE': 'django.db.backends.postgresql_psycopg2',
        'NAME': '<database_name>',
        'USER': 'postgres',
        'PASSWORD': '',
        'HOST': 'localhost',
        'PORT': '5432',
    },
    'mysql': {
        'ENGINE': 'django.db.backends.mysql',
        'NAME': '<database_name>',
        'USER': 'root',
        'PASSWORD': '',
        'HOST': 'localhost',
        'PORT': '3306',
    },
}

# assume mysql server has been running
# centos: yum install postgresql94-server postgresql94-devel
# ubuntu: apt-get install postgresql-9.4 libpq-dev
# service postgresql-9.4 initdb
# service postgresql-9.4 start
# psql -U postgres -c 'create database <database_name>'

# pip install psycopg2
# python manage.py migrate
# python manage.py shell < django_mysql2postgres
"""


def migrate_model(label, mod):
    for item in mod.objects.db_manager(label).all():
        print item
        item.save()


def migrate_model_ex(label, mod, self_ref_fields):
    for item in mod.objects.db_manager(label).all():
        for field in self_ref_fields:
            setattr(item, field, None)
        print item
        item.save()
    for item in mod.objects.db_manager(label).all():
        changed = False
        for field in self_ref_fields:
            if getattr(item, field):
                changed = True
                break
        if changed:
            print item, "ref updated"
            item.save()


def migrate_model_subfields(label, mod, subfields):
    for item in mod.objects.db_manager(label).all():
        for field in subfields:
            relation = getattr(item, field)
            for one in relation.db_manager(label).all():
                print field, one
                relation.add(one)


def special_update_id_seq(tables):
    from django.db import connection
    cur = connection.cursor()
    for table in tables:
        cur.execute(
            'select id from %s order by id desc limit 1' % table
        )
        max_id = cur.fetchone()
        if max_id is None:
            print table, "skipped"
            continue
        max_id = max_id[0]
        print table, max_id + 1
        cur.execute(
            'alter sequence %s_id_seq restart with %d' % (
                table,
                max_id + 1
            )
        )
    cur.close()


from datetime import datetime
start = datetime.now()


from django.contrib.contenttypes.models import ContentType
from django.contrib.auth.models import User, Group, Permission

migrate_model('mysql', User)
migrate_model('mysql', Group)
ContentType.objects.all().delete()
migrate_model('mysql', ContentType)
migrate_model('mysql', Permission)
migrate_model_subfields('mysql', Group, ['permissions'])
migrate_model_subfields('mysql', User, ['groups', 'user_permissions'])

# migrate other models
# e.g. UserProfile has a field which uses Foreign Key to make reference to self.
#   UserProfile: name=string, manager=UserProfile
#   from models import UserProfile; migrate_model_ex('mysql', UserProfile, ['manager'])

print "time:", datetime.now() - start
