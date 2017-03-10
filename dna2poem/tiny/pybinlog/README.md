# Sync MySQL/MariaDB binary log

```bash
virtualenv env
. env/bin/activiate
pip install -r requirements.txt
# setup mysql as root/root and enable binlog
python sync.py sample
```
