#!/bin/bash

# ref: https://github.com/gem/oq-engine/wiki/Installing-LXC-on-CentOS
# ref: http://wiki.centos.org/HowTos/LXC-on-CentOS6
rpm -ivh http://mirror.nl.leaseweb.net/epel/6/i386/epel-release-6-8.noarch.rpm
yum install lxc lxc-libs lxc-templates bridge-utils libcgroup
service cgconfig start
service cgred start
chkconfig --level 345 cgconfig on
chkconfig --level 345 cgred on
brctl addbr lxcbr0
ifconfig lxcbr0 10.0.3.1 netmask 255.255.255.0
ifconfig lxcbr0 up

touch /etc/sysconfig/network-scripts/ifcfg-lxcbr0
# append to /etc/sysconfig/network-scripts/ifcfg-lxcbr0
#   DEVICE="lxcbr0"
#   TYPE="Bridge"
#   BOOTPROTO="static"
#   IPADDR="10.0.3.1"
#   NETMASK="255.255.255.0"

sed "s|-A FORWARD -j REJECT --reject-with icmp-host-prohibited||" /etc/sysconfig/iptables
# append to /etc/sysconfig/iptables
#   *nat
#   :PREROUTING ACCEPT [0:0]
#   :OUTPUT ACCEPT [0:0]
#   :POSTROUTING ACCEPT [0:0]
#   -A POSTROUTING -o eth0 -j MASQUERADE
#   COMMIT
service iptables restart

sed "s|net.ipv4.ip_forward = 0|net.ipv4.ip_forward = 1|" /etc/sysctl.conf
