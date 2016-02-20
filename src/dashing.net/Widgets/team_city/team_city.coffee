class Dashing.TeamCity extends Dashing.Widget

  onData: (data) ->
    # clear existing "status-*" classes
    $(@get('node')).attr 'class', (i,c) ->
      c.replace /\bstatus-\S+/g, ''
      
    if data.status
      # add new class
      $(@get('node')).addClass "status-#{data.status}"