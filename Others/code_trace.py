# encoding: utf-8

# usage (thread-specific):
#  import code_trace

# ref:
#  https://pymotw.com/2/sys/tracing.html
#  https://pymotw.com/2/profile/index.html#module-profile
#  http://www.dalkescientific.com/writings/diary/archive/2005/04/20/tracing_python_code.html
#  http://nedbatchelder.com/blog/200804/wicked_hack_python_bytecode_tracing.html

import sys
import os


ME_DIR = os.path.join(os.path.abspath(os.path.dirname(__file__)), "")
ME_DIR_N = len(ME_DIR)


def _frame_deep(f_back):
    n = 0
    while f_back:
        f_back = f_back.f_back
        n += 1
    return n


def _trace_calls_and_returns(frame, event, arg):
    co = frame.f_code
    func_name = co.co_name
    if func_name == "write":
        # Ignore write() calls from print statements
        return
    line_no = frame.f_lineno
    filename = co.co_filename
    if ME_DIR not in filename:
        return
    
    filename = filename[ME_DIR_N:]
    frame_deep = _frame_deep(frame.f_back) * 3
    if event == "call":
        print "%s<F:%s :: %s(%s)>" % (" " * frame_deep, func_name, filename, line_no)
        return _trace_calls_and_returns
    elif event == "return":
        print "%s<F:%s :: return>" % (" " * frame_deep, func_name) # %s -> arg => return_val
    return


sys.settrace(_trace_calls_and_returns)
